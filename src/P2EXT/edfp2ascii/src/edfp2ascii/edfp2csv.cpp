#include "edfp/edfp.hpp"

int main(int argc, char const *argv[])
{
	EDFP edfp;

	edfp.read_file(argv[1]);
	edfp.csv();

	return 0;
}