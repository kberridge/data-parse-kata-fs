**A flat file data parsing exercise in fsharp.**

Given a flat file parse the tables and their fields into some workable structure.

The flat file looks like:

ADD TABLE "blah"
  DESCRIPTION blah

ADD FIELD "blah" "blah" "blah"
  ORDER 10
  NAME "f1"
  TYPE "varchar"

ADD FIELD "yadda"
  NAME "f2"
  ORDER 5

ADD TABLE "blah2"
  DESCRIPTION blah

ADD FIELD "blah2" "blah2" "blah2"
  ORDER 10
  NAME "f1"
  TYPE "varchar"
