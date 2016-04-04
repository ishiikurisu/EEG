package sst

import "fmt"
import "os"

func GetAnalysisParameters() []string {
	return []string {
		"totalGo",
		"correctGo",
		"totalStop",
		"correctStop",
		"%INHIB",
		"%AUS",
		"SSD",
		"RT",
		"SSRT",
	}
}

func GetResultParameters() []string {
	return []string {
		"RT",
		"SSD",
		"SSRT",
		"%INHIB",
		"%AUS",
	}	
}

/**
 * <p> Generates an analysis structure. Consists of a map relating a parameter
 *     to an array of numbers. The needed parameters can be obtained through the 
 *     "GetAnalysisParameters() string" function.
 * </p>
 * @return the analysis structure
 */
func BeginAnalysis() map[string][]float64 {
	data := make(map[string][]float64) 

	for _, param := range GetAnalysisParameters() {
		data[param] = make([]float64, 0)
	}

	return data
}

/**
 * Updates a multiple analysis structure
 * @param inlet the structure to increment data
 * @param outlet the structure that will be incremented
 * @return outlet with the appropriate changes
 */
func UpdateAnalysis(inlet map[string]float64, 
	                outlet map[string][]float64) map[string][]float64 {
	for _, param := range GetAnalysisParameters() {
		outlet[param] = append(outlet[param], inlet[param])
	}

	return outlet
}

/**
 * Calls the calculations functions upon the analysis structure
 * @param analysis <p> the arrays containing the results of the
 *                     individual analysis </p>
 * @return a map relating the requested paramters and strings of results
 */
func EndAnalysis(analysis map[string][]float64) map[string]string {
	outlet := make(map[string]string)

	for key, value := range analysis {
		mean := Mean(value)
		dev := StdDev(value)
		outlet[key] = fmt.Sprintf("%s: %3f +- %3f", key, mean, dev)
	}

	return outlet
}

/**
 * Formats the output of an analysis structure
 * @param data the analysis structure
 * @return a string containing the formatted structure
 */
func FormatSingle(data map[string]float64) (box string) {
	for _, param := range GetAnalysisParameters() {
		box += fmt.Sprintf("%s: %3f\n", param, data[param])
	}
	return 
}

/**
 * Formats the output of the complete analysis structure
 * @param inlet the result of the analysis
 * @return a formatted string as requested
 */
func FormatMultiple(inlet map[string]string) (box string) {
	for _, item := range GetResultParameters() {
		box += fmt.Sprintf("%s\n", inlet[item])
	}

	return
}

/**
 * Writes data of an analysis structure to a stream
 * @param outlet pointer to the chosen file. if nil, writes to stdout
 * @param data the string to be written on the stream
 */
func Write(outlet *os.File, data string) {
	if outlet == nil {
		fmt.Printf("%s", data)
	} else {
		fmt.Fprintf(outlet, "%s", data)
	}
}