using AOC_2024;
using AoCHelper;

await Solver.SolveAll(options =>
{
    options.ShowConstructorElapsedTime = true;
    options.ShowOverallResults = true;
    options.ClearConsole = false;
});

/* TODO:
 * 
 * optims (anything above 100ms):
 * 
 * │ Day 20 │ Part 2  │ 667 ms 
 * │ Day 22 │ Part 2  │ 242 ms 
 * │ Day 9  │ Part 2  │ 210 ms 
 * │ Day 6  │ Part 2  │ 169 ms
 * │ Day 14 │ Part 2  │ 161 ms 
 * │ Day 17 │ Part 2  │ 126 ms 
 * 
 */

/*await Solver.Solve([typeof(Day25)], options =>
{
    options.ShowConstructorElapsedTime = true;
    options.ShowOverallResults = true;
    options.ClearConsole = true;
});*/
