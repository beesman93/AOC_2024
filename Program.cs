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
 *  * optimize Day18  part 2                (~1.5s)
 *  -- AStar maybe
 *  -- can check for 1024 obsticles vs all obsticles
 *  -- while tail obsticle set is false half down between end and 1024
 *  -- when good maze half up to last bad maze
 *  -- log(n) maze tries this with basically binary search
 * 
 * medium prio optims                       (solves 200ms - 1s)
 *  *  optimize Day09 part 2                (223ms)
 *  -- part 1 can use the part 2 rewrite
 *  -- make the psudo memory not super bad
 *  
 * low prio optims                          (solves 50ms - 200ms)
 *  *   optimize Day06 part 2 further       (187ms)
 *  *   optimize Day14 part 2               (159ms)
 */

await Solver.Solve([typeof(Day18)], options =>
{
    options.ShowConstructorElapsedTime = true;
    options.ShowOverallResults = true;
    options.ClearConsole = true;
});
