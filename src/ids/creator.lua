local creator = { }

--[[ Latex functions ]]

-- Creates the standard header of a LaTeX file
creator.createHeader = function()
  return [[
\documentclass[12pt]{article}
\usepackage[a4paper]{geometry}
\usepackage[cm]{fullpage}
\usepackage[latin1]{inputenc}
\usepackage{longtable}
\usepackage{tabularx}
\usepackage{graphicx}

\begin{document}
\begin{center}
\sffamily
]]
end

-- Creates the standard ending of a LaTeX file
creator.createTail = function()
  return [[

\end{center}
\end{document}
]]
end

-- [[ IO functions ]]

--- Loads a file to its raw string
creator.loadData = function(source)
  local lines = { }

  for line in io.lines(source) do
    lines[#lines + 1] = line
  end

  return table.concat(lines, '\n')
end

-- Builds a table from a TSV string
creator.buildTable = function(raw)
  local outlet = { }
  local data = { }
  local labels = { }
  local lines = strsplit(raw, '\n')
  local limit = -1

  -- Extracting fields from raw data
  for _, line in pairs(lines) do
    local columns = strsplit(line, '\t')
    table.insert(data, columns)
  end

  -- Building map
  labels = data[1]
  limit = #data - 1

  for j = 2, limit do
    for i, label in pairs(labels)  do
      if outlet[label] == nil then
        outlet[label] = { }
      end
      table.insert(outlet[label], data[j][i])
    end
  end

  return outlet
end

-- Turns a hashmap into a string
creator.table2string = function(matrix)
  local outlet = { }

  for tag, line in pairs(matrix) do
    table.insert(outlet, tag .. ": " .. table.concat(line, "; "))
  end

  return table.concat(outlet, '\n')
end

creator.table2latex = function(data)
  local text = "\\begin{longtable}{ | p{3cm}  p{5cm} | p{3cm}  p{5cm} | }\n\\hline\n"
  local k = 1
  local limit = -1

  -- Discover limit
  for tag, stuff in pairs(data) do
    if #stuff > limit then
      limit = #stuff
    end
  end

  -- Iterate over each participant
  for j = 1, limit do
    local sep = " &"
    local id = "\\raisebox{-\\totalheight}{\\includegraphics[width=3cm]{logo.png}} & "

    -- Building identification
    id = id .. data["Universidade"][k] .. " \\newline "
    id = id .. "Nome: " .. data["Nome"][k] .. " \\newline "
    id = id .. "Projeto: " .. data["Projeto"][k] .. " \\newline "
    id = id .. "Responsável: " .. data["Coordenadores"][k]

    -- Determining separator
    if j % 2 == 0 then
      sep = " \\\\ \\hline\n"
    end

    -- Preparing for next step
    text = text .. " " .. id .. sep
    k = k + 1
  end

  -- Adding missing horizontal line, if needed
  if limit % 2 == 1 then
    text = text .. " & \\\\ \\hline\n"
  end

  text = text .. "\\end{longtable}\n"
  return text
end

-- [[ Main functions ]]

-- Splits a string into its fields separated by a separator
function strsplit(inputstr, sep)
  -- http://stackoverflow.com/questions/1426954/split-string-in-lua
  local t = {}; i = 1
  if sep == nil then sep = "%s" end
  for str in string.gmatch(inputstr, "([^"..sep.."]+)") do
    t[i] = str
    i = i + 1
  end
  return t
end

-- The module's main function
creator.create = function(source)
  local outlet = creator.createHeader()
  local raw = creator.loadData(source)
  local data = creator.buildTable(raw)

  -- Formatting data
  -- print("% " .. creator.table2string(data))
  outlet = outlet .. creator.table2latex(data) .. '\n'

  -- Finishing file
  outlet = outlet .. creator.createTail()
  return outlet
end

return creator
