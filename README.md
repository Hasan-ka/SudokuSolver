# Sudoku Solver Backtrack
This program solve any 3x3 Sudoku table using backtrack algorithm.
# How to use?
First you put any 3x3 table you wish to solve into the 2d array simply named sudoku in the MainWindow.xaml.cs file, keep in mind 0 means blank, then hit run and watch the magic happen.
# How to make the solving process faster
I made a choice to make the ui update with change in the sudoku array, although it slows down solving time and fills memory, it's great way to show the how the algorithm work visually, but you can in MainWindow.xaml.cs file enable the function printScreen() located in solvebtn and make sure to disable printSceen() in the solve function to speed solving greatly.
