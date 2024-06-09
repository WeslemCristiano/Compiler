using System;
using System.Collections.Generic;

namespace Compiler
{
    public class SymbolTable
    {
        private List<Entry> _symbols;

        public SymbolTable()
        {
            _symbols = new List<Entry>();
        }

        public int AddSymbol(string varName)
        {
            for (int i = 0; i < _symbols.Count; i++)
            {
                if (_symbols[i].VarName == varName)
                {
                    return i;
                }
            }

            _symbols.Add(new Entry { VarName = varName, Value = null });
            return _symbols.Count - 1;
        }

        public void SetSymbol(string varName, double value)
        {
            for (int i = 0; i < _symbols.Count; i++)
            {
                if (_symbols[i].VarName == varName)
                {
                    _symbols[i] = new Entry { VarName = varName, Value = value };
                    return;
                }
            }

            throw new Exception($"Símbolo '{varName}' não encontrado.");
        }

        public Entry GetSymbol(int index)
        {
            if (index >= 0 && index < _symbols.Count)
            {
                return _symbols[index];
            }

            throw new IndexOutOfRangeException("Índice fora do intervalo da tabela de símbolos.");
        }

        public double GetSymbol(string varName)
        {
            for (int i = 0; i < _symbols.Count; i++)
            {
                if (_symbols[i].VarName == varName)
                {
                    if (_symbols[i].Value.HasValue)
                    {
                        return _symbols[i].Value.Value;
                    }
                    else
                    {
                        throw new Exception($"Símbolo '{varName}' não inicializado.");
                    }
                }
            }

            throw new Exception($"Símbolo '{varName}' não encontrado.");
        }
    }

    public struct Entry
    {
        public string VarName;
        public double? Value;
    }
}
