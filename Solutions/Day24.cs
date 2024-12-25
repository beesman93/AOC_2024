using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AOC_2024
{
    internal class Day24 : BaseDayWithInput
    {
        enum OP
        {
            AND,
            OR,
            XOR
        }
        class Wire
        {
            public string id;
            public bool? Value { get; set; }
            public Wire(string id, bool? Value = null) => (this.id,this.Value) = (id,Value);
            public override string ToString() => $"{id} : {Value}";
        }
        struct Component(Wire input1, Wire input2, OP op, string outputId)
        {
            public Wire input1 = input1;
            public Wire input2 = input2;
            public OP op = op;
            public bool? output = null;
            public string outputId = outputId;
            public override string ToString() => $"{input1} {op} {input2} -> {output}";
        }
        Dictionary<string, Wire> wires = [];
        Dictionary<Wire,Component> components = [];
        private Wire GetOrMake(string id)
        {
            if(!wires.ContainsKey(id))
                wires.Add(id, new Wire(id));
            return wires[id];
        }

        private void trySolve(Component c)
        {
            if (c.input1.Value == null)
                trySolve(components[c.input1]);
            if (c.input2.Value == null)
                trySolve(components[c.input2]);
            switch (c.op)
            {
                case OP.AND:
                    c.output = c.input1.Value & c.input2.Value;
                    break;
                case OP.OR:
                    c.output = c.input1.Value | c.input2.Value;
                    break;
                case OP.XOR:
                    c.output = c.input1.Value ^ c.input2.Value;
                    break;
            }
            wires[c.outputId].Value=c.output;
        }
        private void resetWires()
        {
            foreach (var wire in wires.Values)
                wire.Value = null;
        }
        private void setInputs(long val, char prefix)
        {
            foreach (var wire in wires.Where(x => x.Key[0] == prefix))
            {
                bool set = ((val >> Convert.ToInt32(wire.Key.Substring(1)))&1)!=0;
                wire.Value.Value = set;
            }
        }
        private void solveBackwardsZs()
        {
            foreach (var comp in components.Values.Where(x => x.outputId[0] == 'z'))
                trySolve(comp);
        }

        private long getZoutput()
        {
            long ans = 0;
            foreach (var wire in wires.Where(x => x.Key[0] == 'z'))
                if (wire.Value.Value == true)
                    ans = ans + ((long)1 << Convert.ToInt32(wire.Key.Substring(1)));
            return ans;
        }
        public Day24()
        {
            int i = 0;
            while (_input[i] != "")
            {
                var lr = _input[i].Split(": ");
                wires.Add(lr[0], new Wire(lr[0], lr[1] == "1"));
                i++;
            }i++;
            for (; i < _input.Length; i++)
            {
                var lr = _input[i].Split(" -> ");
                var ll = lr[0].Split(" ");
                OP op = ll[1] switch
                {
                    "AND" => OP.AND,
                    "OR" => OP.OR,
                    "XOR" => OP.XOR,
                    _ => throw new Exception("Invalid OP")
                };
                components.Add(GetOrMake(lr[1]), new Component(GetOrMake(ll[0]), GetOrMake(ll[2]), op, lr[1]));
            }
        }
        private string? GetComponent(string? input1, string? input2, OP op)
        {
            foreach(var comp in components.Values)
            {
                if (comp.op == op && (
                   (comp.input1.id == input1 && comp.input2.id == input2)||
                   (comp.input1.id == input2 && comp.input2.id == input1)))
                        return comp.outputId;
            }
            return null;
        }

        private void Swap(string a, string b)
        {
            var comp1 = components[wires[a]];
            var comp2 = components[wires[b]];
            comp1.outputId = b;
            comp2.outputId = a;
            components[wires[a]] = comp2;
            components[wires[b]] = comp1;
        }

        private List<string> GetSwapFixes_AssumingBasicAdder()
        {
            List<string> swapMembers = [];

            string carryWire = GetComponent("x00","y00",OP.AND);
            int bit = 1;

            int maxZ = 0;
            foreach (var wire in wires.Where(x => x.Key[0] == 'z'))
                maxZ = int.Max(Convert.ToInt32(wire.Key.Substring(1)), maxZ);

            while (bit< maxZ)
            {
                string xID = $"x{bit.ToString("00")}";
                string yID = $"y{bit.ToString("00")}";
                string zID = $"z{bit.ToString("00")}";

                var xXORy = GetComponent(xID,yID,OP.XOR);
                var xANDy = GetComponent(xID, yID,OP.AND);
                var xXORy_XOR_CIN = GetComponent(xXORy, carryWire,OP.XOR);

                if (xXORy_XOR_CIN == null)
                {
                    swapMembers.Add(xXORy);
                    swapMembers.Add(xANDy);
                    Swap(xXORy, xANDy);
                    bit = 1;
                    carryWire = GetComponent("x00", "y00", OP.AND);
                    continue;
                }

                if (xXORy_XOR_CIN != zID)
                {
                    swapMembers.Add(xXORy_XOR_CIN);
                    swapMembers.Add(zID);
                    Swap(xXORy_XOR_CIN, zID);
                    bit = 1;
                    carryWire = GetComponent("x00", "y00", OP.AND);
                    continue;
                }

                var XOR_AND = GetComponent(xXORy, carryWire, OP.AND);
                carryWire = GetComponent(XOR_AND,xANDy,OP.OR);
                bit++;
            }
            swapMembers.Sort();
            return swapMembers;
        }
        public override ValueTask<string> Solve_1()
        {
            solveBackwardsZs();
            return new($"{getZoutput()}");
        }
        public override ValueTask<string> Solve_2() => new($"{string.Join(',',GetSwapFixes_AssumingBasicAdder())}");
    }
}
