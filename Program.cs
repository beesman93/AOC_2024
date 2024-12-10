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
 *  
 *  optimize Day09 part 2
 *  -- part 1 can use the part 2 rewrite after
 *  -- make the psudo memory not super bad
 */

Solver.Solve([typeof(Day10)], options =>
{
    options.ShowConstructorElapsedTime = true;
    options.ShowOverallResults = true;
    options.ClearConsole = false;
});
