using AOC_2024;
using AoCHelper;

/*await Solver.SolveAll(options =>
{
    options.ShowConstructorElapsedTime = true;
    options.ShowOverallResults = true;
    options.ClearConsole = false;
});*/

/* TODO:
 * 
 * high prio optims                         (solves >1s)
 *  * nothing to do
 * 
 * medium prio optims                       (solves 300ms - 1s)
 *  *  optimize Day09 part 2                (585ms)
 *      -- part 1 can use the part 2 rewrite after
 *      -- make the psudo memory not super bad
 *  
 * low prio optims                          (solves 50ms - 300ms)
 *  *   optimize Day06 part 2 further       (187ms)
 */

Solver.Solve([typeof(Day06)], options =>
{
    options.ShowConstructorElapsedTime = true;
    options.ShowOverallResults = true;
    options.ClearConsole = false;
});
