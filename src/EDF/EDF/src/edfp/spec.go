package edfp

func GetSpecsLength() map[string]int {
    spec := make(map[string]int)

    spec["version"] = 8
    spec["patient"] = 80
    spec["recording"] = 80
    spec["startdate"] = 8
    spec["starttime"] = 8
    spec["bytesheader"] = 8
    spec["reserved"] = 44
    spec["datarecords"] = 8
    spec["duration"] = 8
    spec["numbersignals"] = 4
    spec["label"] = 16
    spec["transducer"] = 80
    spec["physicaldimension"] = 8
    spec["physicalminimum"] = 8
    spec["physicalmaximum"] = 8
    spec["digitalminimum"] = 8
    spec["digitalmaximum"] = 8
    spec["prefiltering"] = 80
    spec["samplesrecord"] = 8
    spec["chanreserved"] = 32

    return spec
}

func GetSpecsList() []string {
    spec := make([]string, 20)

    spec[0] = "version"
    spec[1] = "patient"
    spec[2] = "recording"
    spec[3] = "startdate"
    spec[4] = "starttime"
    spec[5] = "bytesheader"
    spec[6] = "reserved"
    spec[7] = "datarecords"
    spec[8] = "duration"
    spec[9] = "numbersignals"
    spec[10] = "label"
    spec[11] = "transducer"
    spec[12] = "physicaldimension"
    spec[13] = "physicalminimum"
    spec[14] = "physicalmaximum"
    spec[15] = "digitalminimum"
    spec[16] = "digitalmaximum"
    spec[17] = "prefiltering"
    spec[18] = "samplesrecord"
    spec[19] = "chanreserved"

    return spec
}
