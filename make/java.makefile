sourcefiles = src/github.com/lab-neuro-comp/EDF/java/br/unb/biologiaanimal/edf/EDF.java src/github.com/lab-neuro-comp/EDF/java/br/unb/biologiaanimal/edf/EDFReader.java src/github.com/lab-neuro-comp/EDF/java/br/unb/biologiaanimal/edf/EDFConstants.java src/github.com/lab-neuro-comp/EDF/java/br/unb/biologiaanimal/edf/EDFUtil.java src/github.com/lab-neuro-comp/EDF/java/br/unb/biologiaanimal/edf/EDFWriter.java
classfiles = -C $(src) br/unb/biologiaanimal/edf/EDF.class -C $(src) br/unb/biologiaanimal/edf/EDFReader.class -C $(src) br/unb/biologiaanimal/edf/EDFConstants.class -C $(src) br/unb/biologiaanimal/edf/EDFUtil.class -C $(src) br/unb/biologiaanimal/edf/EDFWriter.class
testfiles = src/github.com/lab-neuro-comp/EDF/test/br/unb/biologiaanimal/test/Test.java
runtestfiles = src/github.com/lab-neuro-comp/EDF/test/br/unb/biologiaanimal/test/Run.java
classname = br.unb.biologiaanimal.edf.EDF
testclassname = br.unb.biologiaanimal.test.Test
runtestclassname = br.unb.biologiaanimal.test.Run
targetname = src/github.com/lab-neuro-comp/EDF/target/edf.jar
manifest = src/github.com/lab-neuro-comp/EDF/manifest.txt
testfolder = src/github.com/lab-neuro-comp/EDF/test
src = src/github.com/lab-neuro-comp/EDF/java

do: jar
	java -jar $(targetname)

classes:
	javac -target 1.4 -source 1.4 -nowarn $(sourcefiles)

try: classes
	java -cp $(src) $(classname)

jar: classes
	jar cvfm $(targetname) $(manifest) $(classfiles)

compiletests:
	javac -cp $(testfiles)

compileruntest:
	javac -cp $(targetname) $(runtestfiles)

test: jar compileruntest
	java -cp $(targetname);$(testfolder) $(runtestclassname)
