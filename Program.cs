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
 * high prio optims                         (solves ~1s+)
 * *   optimize Day20 part 2                (680ms)
 * *   optimize Day22 part 2 more           (850ms)
 * 
 * medium prio optims                       (solves 200ms - ~1s)
 *  *  optimize Day09 part 2                (223ms)
 *  -- part 1 can use the part 2 rewrite
 *  -- make the psudo memory not super bad
 *  
 * low prio optims                          (solves 50ms - 200ms)
 *  *   optimize Day06 part 2 further       (187ms)
 *  *   optimize Day14 part 2               (159ms)
 *  *   optimize Day17 part 2               (153ms)
 */

await Solver.Solve([typeof(Day22)], options =>
{
    options.ShowConstructorElapsedTime = true;
    options.ShowOverallResults = true;
    options.ClearConsole = true;
});
