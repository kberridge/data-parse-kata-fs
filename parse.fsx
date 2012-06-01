open System.Text.RegularExpressions

type Field = { Details : string; Name : string; Order : string; Type : string; }
type Table = { Details : string; Description : string; Fields : Field list }

let linesArr = System.IO.File.ReadAllLines("data.txt")
let lines = Array.toList linesArr

let isBlankLine line =
  line = ""

let nextLine lines =
  if lines = [] then
    "", [] // Return "" when no more lines.  This feels hacky...
  else
    (List.head lines), (List.tail lines)

let (|Match1|_|) pattern input =
  let m = Regex.Match(input, pattern)
  if m.Success then Some(m.Groups.[1].Value) else None

let rec parseFieldLines remainingLines field =
  let line, remainingLines = nextLine remainingLines
  match line with
    | Match1 "ORDER (.+)" order -> parseFieldLines remainingLines { field with Field.Order = order; }
    | Match1 "NAME (.+)" name -> parseFieldLines remainingLines { field with Field.Name = name; }
    | Match1 "TYPE (.+)" _type -> parseFieldLines remainingLines { field with Field.Type = _type; }
    | l when isBlankLine l -> field, remainingLines
    | _ -> parseFieldLines remainingLines field

let parseField remainingLines fieldLine table =
  let m = Regex.Match(fieldLine, "ADD FIELD (.+)")
  let details = m.Groups.[1].Value
  let field = { Field.Details = details; Name = ""; Order = ""; Type = "" }
  parseFieldLines remainingLines field

let rec parseTableLines remainingLines table =
  let line, remainingLines = nextLine remainingLines
  match line with
    | Match1 "DESCRIPTION (.+)" desc -> parseTableLines remainingLines { table with Table.Description = desc; }
    | l when isBlankLine l -> table, remainingLines
    | _ -> parseTableLines remainingLines table

let rec parseTableFields remainingLines table =
  let matchedFieldLine remainingLines fieldLine =
    let field, remainingLines = parseField remainingLines fieldLine table
    let table = { table with Table.Fields = field :: table.Fields }
    parseTableFields remainingLines table

  let line, remainingLines = nextLine remainingLines
  match line with
    | Match1 "ADD FIELD (.+)" _ -> matchedFieldLine remainingLines line
    | Match1 "ADD TABLE" _ -> table, line :: remainingLines // push the line back on
    | l when isBlankLine l -> table, remainingLines
    | _ -> parseTableFields remainingLines table

let parseTable remainingLines tableLine =
  let r = Regex.Match(tableLine, "ADD TABLE (.+)")
  let details = r.Groups.[1].Value
  let table = { Table.Details = details; Description = ""; Fields = [] }

  let table, remainingLines = parseTableLines remainingLines table
  parseTableFields remainingLines table

let rec parseTables remainingLines tables =
  let matchedNonEmpty() =
    let line, remainingLines = nextLine remainingLines
    let table, remainingLines = parseTable remainingLines line
    parseTables remainingLines (table :: tables)
    
  match remainingLines with
    | [] -> tables
    | _ -> matchedNonEmpty()
  
let tables = parseTables lines []
printfn "%A" tables
