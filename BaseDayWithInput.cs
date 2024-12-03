using AoCHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoCHelper;

public abstract class BaseDayWithInput : BaseDay
{
    protected readonly string[] _input;
    public BaseDayWithInput()
    {
        _input = File.ReadAllLines(InputFilePath);
    }
}
