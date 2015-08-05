#ifndef EDFP_H
#define EDFP_H 0

#include <cstdio>
#include <map>
#include <vector>

#include "auxiliar.hpp"
#include "edfP_spec.hpp"
#include "record.hpp"

class EDFPP
{
	std::map<std::string, std::string> header;
	std::vector<DATA_RECORD> data_records;
	size_t number_signals;
	size_t number_data_records;
	float duration;
	void read_header(FILE*);
	void read_data_record(FILE*);

public:
	EDFP(void) {};
	~EDFP(void) {};
	void read_file(const char*);
	void yaml(const char*);
	void csv(void);
	friend class DATA_RECORD;
};

/* HEADER SPEC
	8: version
	80: local patient identification
	80: local recording identification
	8: startdate of recording (dd.mm.yy)
	8: starttime of recording (hh.mm.ss)
	8: number of bytes in header record
	44: reserved (not present in EDF+)
	8: number of data records
	8: duration of a data record in seconds
	4: ns = number of signals in data record
	ns * 16: ns * label
	ns * 80: ns * transducer type
	ns * 8: physical dimension
	ns * 8: physical minimum
	ns * 8: physical maximum
	ns * 8: digital minimum
	ns * 8: digital maximum
	ns * 80: ns * prefiltering
	ns * 8: ns * # of samples in each data record
	ns * 32: ns * reserved
*/
void EDFP::read_header(FILE* inlet)
{
	std::vector<std::string>::iterator it;
	std::string data;
	size_t aux_number;
	size_t bytes;

	it = EDFP_SPECS.begin();
	while ((*it).compare("label") != 0)
	{
		bytes = EDFP_SPEC[*it];
		data = read_to_string(inlet, bytes);
		header[*it] = data;

		if ((*it).compare("reserved") == 0) {
			// analize if it is EDF+ or EDF
		}
		else if ((*it).compare("datarecords") == 0) {
			sscanf(data.c_str(), "%d", &aux_number);
			number_data_records = (size_t) aux_number;
		}
		else if ((*it).compare("duration") == 0) {
			sscanf(data.c_str(), "%f", &duration);
		}
		else if ((*it).compare("numbersignals") == 0) {
			sscanf(data.c_str(), "%d", &aux_number);
			number_signals = (size_t) aux_number;
		}

		++it;
	}

	// allocate memory
	for (size_t r = 0; r < number_signals; ++r)
		data_records.push_back(DATA_RECORD());

	while (it != EDFP_SPECS.end())
	{
		bytes = EDFP_SPEC[*it];
		for (size_t i = 0; i < number_signals; ++i)
		{
			data = read_to_string(inlet, bytes);
			data_records[i].header[*it] = data;

			if ((*it).compare("samplesrecord") == 0) {
				sscanf(data.c_str(), "%d", &aux_number);
				data_records[i].number_samples = (size_t) aux_number;
			}
		}
		++it;
	}
}

/*
# samples * integer: first signal in data record
# samples * integer: second signal
... ns times
*/
void EDFP::read_data_record(FILE* inlet)
{
	for (size_t i = 0; i < number_signals; ++i)
		data_records[i].read_record(inlet, duration);
}

void EDFP::read_file(const char* input)
{
	FILE* inlet = NULL;

	if (input == NULL)
		inlet = stdin;
	else
		inlet = fopen(input, "rb");

	EDF_SETUP();
	read_header(inlet);
	for (size_t k = 0; k < number_data_records; ++k)
		read_data_record(inlet);

	fclose(inlet);
}

void EDFP::yaml(const char *output)
{
	std::vector<std::string>::iterator it;
	std::vector<std::string>::iterator checkpoint;
	DATA_RECORD data_record;
	FILE *outlet = stdout;
	size_t i;

	if (output != NULL)
		outlet = fopen(output, "wb");
		

	// write header's header
	it = EDFP_SPECS.begin();
	while ((*it).compare("label"))
	{
	   write_from_string(outlet, *it);
	   write_from_string(outlet, ": ");
	   write_from_string(outlet, header[*it]);
	   write_from_string(outlet, "\n");
	   ++it;
	}

	// write records' header
	while (it != EDFP_SPECS.end())
	{
		write_from_string(outlet, *it);
		write_from_string(outlet, ":\n");
		for (i = 0; i < number_signals; ++i)
		{
			write_from_string(outlet, "- ");
			write_from_string(outlet, data_records[i].header[*it]);
			write_from_string(outlet, "\n");
		}
		++it;
	}

	// write records' records
	fprintf(outlet, "Signals:\n");
	for (i = 0; i < number_signals; ++i)
	{
		fprintf(outlet, "  %d: ", i);
		std::vector<short> record = data_records[i].record;
		for (std::vector<short>::iterator r = record.begin(); r != record.end(); ++r)
			fprintf(outlet, "%d\t", *r);
		fprintf(outlet, "\n");
	}

	if (output != NULL)
		fclose(outlet);
}

void EDFP::csv()
{
	std::vector< std::vector<float> > records;
	std::vector<float> record;
	unsigned long limit = -1;
	size_t i, j;
	float data;

	// write header
	printf("title:%s,", header["recording"].c_str());
	printf("recorded:%s %s,",
		header["startdate"].c_str(), header["starttime"].c_str());
	printf("sampling:128,");
	printf("subject:%s,", header["patient"].c_str());
	printf("labels:");
	for (i = 0; i < number_signals; ++i)
		printf("%s ", data_records[i].header["label"].c_str()); printf(",");
	printf("chan:%d,", number_signals);
	printf("units:emotiv\n");

	// write data records
	for (i = 0; i < number_signals; ++i)
		records.push_back(data_records[i].get_translated_record());
	limit = records.at(0).size();
	for (j = 0; j < limit; ++j)
	{
		for (i = 0; i < number_signals; ++i)
		{
			record = records.at(i);
			data = record.at(j);
			if (i == 0) printf("%f", data);
			else printf(", %f", data);
		}
		printf("\n");
	}
	records.clear();
}


#endif