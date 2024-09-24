# Fuudoku
This is small project, to stop being stuck on sudokus for a whole day, when there is plenty more interesting stuff, that needs to be done. From all riddles sudoku is the worst - it's just not intended for humans. 
WTF is that, just search in the <b>arrays of numbers</b>  and remove some from the possibilities. Once again it's not for humans.

# How to use
To run Fuuudoku.CLI.exe you need to provide sudoku definition. For now only option is with a string where `.` are empty fields, like this:<br>
```
..9.85....6......9.78....14...........5.18......7..482.....7.4.2..6.9....8.....7.
```
You can provide it directly in command line with `--d`
```
Fuuudoku.CLI.exe --d ..9.85....6......9.78....14...........5.18......7..482.....7.4.2..6.9....8.....7.
```
or via path to file, that contains the definition with `--p`
```
Fuuudoku.CLI.exe --p C:\I\dont\like\sudokus
```
