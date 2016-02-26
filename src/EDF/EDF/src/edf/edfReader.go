package edf

import "os"
import "fmt"
import "bytes"
import "encoding/binary"

/* --- UTILITY FUNCTIONS --- */
func append(original, to_append []int16) []int16 {
    lo := len(original)
    lt := len(to_append)
    outlet := make([]int16, lo + lt)

    for o := 0; o < lo; o++ {
        outlet[o] = original[o]
    }
    for t := 0; t < lt; t++ {
        outlet[lo+t] = to_append[t]
    }

    return outlet
}

func translate(inlet []byte) []int16 {
    var data int16
    limit := len(inlet)/2
    outlet := make([]int16, limit)
    buffer := bytes.NewReader(inlet)

    for i := 0; i < limit; i++ {
        // shit := binary.Read(buffer, binary.BigEndian, &data)
        shit := binary.Read(buffer, binary.LittleEndian, &data)
        if shit == nil {
            outlet[i] = data
        }
    }

    return outlet
}

/* --- AUXILIAR FUNCTIONS --- */
func str2int(inlet string) int {
    var outlet int = 0
    fmt.Sscanf(inlet, "%d", &outlet)
    return outlet
}

func getNumberSignals(header map[string]string) int {
    return str2int(header["numbersignals"])
}

func getNumberSamples(header map[string]string) []int {
    numberSignals := getNumberSignals(header)
    numberSamples := make([]int, numberSignals)
    samples := header["samplesrecord"]
    sampleSize := len(samples) / numberSignals

    for i := 0; i < numberSignals; i++ {
        numberSamples[i] = str2int(samples[sampleSize*i:sampleSize*i+sampleSize-1])
    }

    return numberSamples
}

/* --- MAIN FUNCTIONS --- */
/**
 * Reads an EDF file
 * @param input a path to the file
 * @return header a map containing the EDF's header
 * @return records a matrix containing the data records
 */
func ReadFile(input string) (map[string]string, [][]int16) {
    inlet, _ := os.Open(input)
    specsList := GetSpecsList()
    specsLength := GetSpecsLength()

    defer inlet.Close()
    header := ReadHeader(inlet, specsList, specsLength)
    records := ReadRecords(inlet, header)

    return header, records
}

/**
 * Reads the header of an EDF file
 * @param inlet file pointer to the EDF
 * @param specsList an array of strings containing the header itens in order
 * @param specsLength a map containing how many bytes each field occupy
 * @return header a map containing each field
 */
func ReadHeader(inlet *os.File, specsList []string, specsLength map[string]int) map[string]string {
    header := make(map[string]string)
    index := 0

    for index < len(specsList) {
        spec := specsList[index]

        if spec == "label" {
            break
        } else {
            data := make([]byte, specsLength[spec])
            n, _ := inlet.Read(data)
            header[spec] = string(data[:n])
        }

        index++
    }

    numberSignals := getNumberSignals(header)
    for index = index; index < len(specsList) ; index++ {
        spec := specsList[index]
        data := make([]byte, specsLength[spec] * numberSignals)
        n, _ := inlet.Read(data)
        header[spec] = string(data[:n])
    }

    return header
}

/**
 * Reads the data records from the file
 * @param inlet the source file
 * @param header the map generated by the function readHeader
 * @return a matrix containing the data records
 */
func ReadRecords(inlet *os.File, header map[string]string) [][]int16 {
    numberSignals := getNumberSignals(header)
    numberSamples := getNumberSamples(header)
    records := make([][]int16, numberSignals)
    sampling := make([]int, numberSignals)
    duration := str2int(header["duration"])
    dataRecords := str2int(header["datarecords"])

    // setup records
    for i := 0; i < numberSignals; i++ {
        sampling[i] = duration * numberSamples[i]
        records[i] = make([]int16, sampling[i])
    }

    // translate data
    for d := 0; d < dataRecords; d++ {
        for i := 0; i < numberSignals; i++ {    
            data := make([]byte, 2*sampling[i])
            inlet.Read(data)
            records[i] = append(records[i], translate(data))
        }
    }

    return records
}