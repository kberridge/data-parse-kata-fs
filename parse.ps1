$lines = gc data.txt

for ($i = 0; $i -le $lines.Length; $i++) {
  $line = $lines[$i]
  if ($line -like 'ADD*') {
    write-host "line with ADD*: $line"
  }
}
