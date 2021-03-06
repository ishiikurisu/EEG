# Command cgo

+ Using cgo with the go command
+ Go references to C
+ C references to Go
+ Using cgo directly
+ Cgo enables the creation of Go packages that call C code.

Using cgo with the go command
-----------------------------

To use cgo write normal Go code that imports a pseudo-package "C". The Go code can then refer to types such as C.size_t, variables such as C.stdout, or functions such as C.putchar.

If the import of "C" is immediately preceded by a comment, that comment, called the preamble, is used as a header when compiling the C parts of the package. For example:

``` go
// #include <stdio.h>
// #include <errno.h>
import "C"
```

The preamble may contain any C code, including function and variable declarations and definitions. These may then be referred to from Go code as though they were defined in the package "C". All names declared in the preamble may be used, even if they start with a lower-case letter. Exception: static variables in the preamble may not be referenced from Go code; static functions are permitted.

See $GOROOT/misc/cgo/stdio and $GOROOT/misc/cgo/gmp for examples. See ["C? Go? Cgo!"](https://golang.org/doc/articles/c_go_cgo.html) for an introduction to using cgo.

CFLAGS, CPPFLAGS, CXXFLAGS and LDFLAGS may be defined with pseudo #cgo directives within these comments to tweak the behavior of the C or C++ compiler. Values defined in multiple directives are concatenated together. The directive can include a list of build constraints limiting its effect to systems satisfying one of the constraints (see [](https://golang.org/pkg/go/build/#hdr-Build_Constraints) for details about the constraint syntax). For example:

``` go
// #cgo CFLAGS: -DPNG_DEBUG=1
// #cgo amd64 386 CFLAGS: -DX86=1
// #cgo LDFLAGS: -lpng
// #include <png.h>
import "C"
```

Alternatively, CPPFLAGS and LDFLAGS may be obtained via the pkg-config tool using a '#cgo pkg-config:' directive followed by the package names. For example:

``` go
// #cgo pkg-config: png cairo
// #include <png.h>
import "C"
```

When building, the CGO_CFLAGS, CGO_CPPFLAGS, CGO_CXXFLAGS and CGO_LDFLAGS environment variables are added to the flags derived from these directives. Package-specific flags should be set using the directives, not the environment variables, so that builds work in unmodified environments.

All the cgo CPPFLAGS and CFLAGS directives in a package are concatenated and used to compile C files in that package. All the CPPFLAGS and CXXFLAGS directives in a package are concatenated and used to compile C++ files in that package. All the LDFLAGS directives in any package in the program are concatenated and used at link time. All the pkg-config directives are concatenated and sent to pkg-config simultaneously to add to each appropriate set of command-line flags.

When the cgo directives are parsed, any occurrence of the string ${SRCDIR} will be replaced by the absolute path to the directory containing the source file. This allows pre-compiled static libraries to be included in the package directory and linked properly. For example if package foo is in the directory /go/src/foo:

``` go
// #cgo LDFLAGS: -L${SRCDIR}/libs -lfoo
```

Will be expanded to:

``` go
// #cgo LDFLAGS: -L/go/src/foo/libs -lfoo
```

When the Go tool sees that one or more Go files use the special import "C", it will look for other non-Go files in the directory and compile them as part of the Go package. Any .c, .s, or .S files will be compiled with the C compiler. Any .cc, .cpp, or .cxx files will be compiled with the C++ compiler. Any .h, .hh, .hpp, or .hxx files will not be compiled separately, but, if these header files are changed, the C and C++ files will be recompiled. The default C and C++ compilers may be changed by the CC and CXX environment variables, respectively; those environment variables may include command line options.

The cgo tool is enabled by default for native builds on systems where it is expected to work. It is disabled by default when cross-compiling. You can control this by setting the CGO_ENABLED environment variable when running the go tool: set it to 1 to enable the use of cgo, and to 0 to disable it. The go tool will set the build constraint "cgo" if cgo is enabled.

When cross-compiling, you must specify a C cross-compiler for cgo to use. You can do this by setting the CC_FOR_TARGET environment variable when building the toolchain using make.bash, or by setting the CC environment variable any time you run the go tool. The CXX_FOR_TARGET and CXX environment variables work in a similar way for C++ code.

Go references to C
------------------

Within the Go file, C's struct field names that are keywords in Go can be accessed by prefixing them with an underscore: if x points at a C struct with a field named "type", `x._type` accesses the field. C struct fields that cannot be expressed in Go, such as bit fields or misaligned data, are omitted in the Go struct, replaced by appropriate padding to reach the next field or the end of the struct.

The standard C numeric types are available under the names C.char, C.schar (signed char), C.uchar (unsigned char), C.short, C.ushort (unsigned short), C.int, C.uint (unsigned int), C.long, C.ulong (unsigned long), C.longlong (long long), C.ulonglong (unsigned long long), C.float, C.double. The C type void* is represented by Go's unsafe.Pointer.

To access a struct, union, or enum type directly, prefix it with struct_, union_, or enum_, as in C.struct_stat.

As Go doesn't have support for C's union type in the general case, C's union types are represented as a Go byte array with the same length.

Go structs cannot embed fields with C types.

Cgo translates C types into equivalent unexported Go types. Because the translations are unexported, a Go package should not expose C types in its exported API: a C type used in one Go package is different from the same C type used in another.

Any C function (even void functions) may be called in a multiple assignment context to retrieve both the return value (if any) and the C errno variable as an error (use _ to skip the result value if the function returns void). For example:

``` go
n, err := C.sqrt(-1)
_, err := C.voidFunc()
```

Calling C function pointers is currently not supported, however you can declare Go variables which hold C function pointers and pass them back and forth between Go and C. C code may call function pointers received from Go. For example:

``` go
package main

// typedef int (*intFunc) ();
//
// int
// bridge_int_func(intFunc f)
// {
//		return f();
// }
//
// int fortytwo()
// {
//	    return 42;
// }
import "C"
import "fmt"

func main() {
	f := C.intFunc(C.fortytwo)
	fmt.Println(int(C.bridge_int_func(f)))
	// Output: 42
}
```

In C, a function argument written as a fixed size array actually requires a pointer to the first element of the array. C compilers are aware of this calling convention and adjust the call accordingly, but Go cannot. In Go, you must pass the pointer to the first element explicitly: C.f(&C.x[0]).

A few special functions convert between Go and C types by making copies of the data. In pseudo-Go definitions:

``` go
// Go string to C string
// The C string is allocated in the C heap using malloc.
// It is the caller's responsibility to arrange for it to be
// freed, such as by calling C.free (be sure to include stdlib.h
// if C.free is needed).
func C.CString(string) *C.char

// C string to Go string
func C.GoString(*C.char) string

// C string, length to Go string
func C.GoStringN(*C.char, C.int) string

// C pointer, length to Go []byte
func C.GoBytes(unsafe.Pointer, C.int) []byte
```

C references to Go
------------------

Go functions can be exported for use by C code in the following way:

``` go
//export MyFunction
func MyFunction(arg1, arg2 int, arg3 string) int64 {...}

//export MyFunction2
func MyFunction2(arg1, arg2 int, arg3 string) (int64, *C.char) {...}
```

They will be available in the C code as:

``` c
extern int64 MyFunction(int arg1, int arg2, GoString arg3);
extern struct MyFunction2_return MyFunction2(int arg1, int arg2, GoString arg3);
```

found in the `_cgo_export.h` generated header, after any preambles copied from the cgo input files. Functions with multiple return values are mapped to functions returning a struct. Not all Go types can be mapped to C types in a useful way.

Using `//export` in a file places a restriction on the preamble: since it is copied into two different C output files, it must not contain any definitions, only declarations. If a file contains both definitions and declarations, then the two output files will produce duplicate symbols and the linker will fail. To avoid this, definitions must be placed in preambles in other files, or in C source files.

Using cgo directly
------------------

Usage:

``` bash
go tool cgo [cgo options] [-- compiler options] gofiles...
```

Cgo transforms the specified input Go source files into several output Go and C source files.

The compiler options are passed through uninterpreted when invoking the C compiler to compile the C parts of the package.

The following options are available when running cgo directly:

```
-dynimport file
	Write list of symbols imported by file. Write to
	-dynout argument or to standard output. Used by go
	build when building a cgo package.
-dynout file
	Write -dynimport output to file.
-dynpackage package
	Set Go package for -dynimport output.
-dynlinker
	Write dynamic linker as part of -dynimport output.
-godefs
	Write out input file in Go syntax replacing C package
	names with real values. Used to generate files in the
	syscall package when bootstrapping a new target.
-objdir directory
	Put all generated files in directory.
-importpath string
	The import path for the Go package. Optional; used for
	nicer comments in the generated files.
-exportheader file
	If there are any exported functions, write the
	generated export declarations to file.
	C code can #include this to see the declarations.
-gccgo
	Generate output for the gccgo compiler rather than the
	gc compiler.
-gccgoprefix prefix
	The -fgo-prefix option to be used with gccgo.
-gccgopkgpath path
	The -fgo-pkgpath option to be used with gccgo.
-import_runtime_cgo
	If set (which it is by default) import runtime/cgo in
	generated output.
-import_syscall
	If set (which it is by default) import syscall in
	generated output.
-debug-define
	Debugging option. Print #defines.
-debug-gcc
	Debugging option. Trace C compiler execution and output.
```
