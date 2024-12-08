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
 * optimize Day06 part 2
 *  -- cache right turns without interference from new obsticle
 *  -- call cache unless new obsticle is in the way
 */

Solver.Solve([typeof(Day08)]);
