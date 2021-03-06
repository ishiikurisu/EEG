import spectro

__ABOUT__ = """
This script is a line-filter for generating spectrograms from ASCII files. It
is expected these files have already been filtered from MATLAB.
"""

def read():
    outlet = { }

    while True:
        try:
            line = map(float, input().strip().split('\t'))
            for i, it in enumerate(line):
                if i not in outlet:
                    outlet[i] = [ ]
                outlet[i].append(it)
        except EOFError:
            break

    return outlet

if __name__ == '__main__':
    data = read()
    spectro.generate_plots(data)
