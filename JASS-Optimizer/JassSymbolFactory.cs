using JassOptimizer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace JASS_Optimizer
{
    internal static class JassSymbolFactory
    {
        internal static JassSymbol Get(string name)
        {
            var result = name switch
            {
                "and" => JassSymbol.AND,
                "array" => JassSymbol.ARRAY,
                "boolean" => JassSymbol.BOOLEAN,
                "code" => JassSymbol.CODE,
                "call" => JassSymbol.CALL,
                "constant" => JassSymbol.CONSTANT,
                "debug" => JassSymbol.DEBUG,
                "else" => JassSymbol.ELSE,
                "elseif" => JassSymbol.ELSEIF,
                "endfunction" => JassSymbol.ENDFUNCTION,
                "endif" => JassSymbol.ENDIF,
                "endloop" => JassSymbol.ENDLOOP,
                "endglobals" => JassSymbol.ENDGLOBALS,
                "extends" => JassSymbol.EXTENDS,
                "exitwhen" => JassSymbol.EXITWHEN,
                "false" => JassSymbol.FALSE,
                "function" => JassSymbol.FUNCTION,
                "globals" => JassSymbol.GLOBALS,
                "handle" => JassSymbol.HANDLE,
                "if" => JassSymbol.IF,
                "integer" => JassSymbol.INTEGER,
                "local" => JassSymbol.LOCAL,
                "loop" => JassSymbol.LOOP,
                "native" => JassSymbol.NATIVE,
                "not" => JassSymbol.NOT,
                "nothing" => JassSymbol.NOTHING,
                "null" => JassSymbol.NULL,
                "or" => JassSymbol.OR,
                "real" => JassSymbol.REAL,
                "returns" => JassSymbol.RETURNS,
                "return" => JassSymbol.RETURN,
                "set" => JassSymbol.SET,
                "string" => JassSymbol.STRING,
                "takes" => JassSymbol.TAKES,
                "then" => JassSymbol.THEN,
                "true" => JassSymbol.TRUE,
                "type" => JassSymbol.TYPE,
                "," => JassSymbol.COMMA,
                "==" => JassSymbol.EQ_EQ,
                "=" => JassSymbol.EQ,
                "!=" => JassSymbol.NEQ,
                "<=" => JassSymbol.LT_EQ,
                "<" => JassSymbol.LT,
                ">=" => JassSymbol.GT_EQ,
                ">" => JassSymbol.GT,
                "+" => JassSymbol.PLUS,
                "-" => JassSymbol.MINUS,
                "*" => JassSymbol.MUL,
                "/" => JassSymbol.DIV,
                "(" => JassSymbol.LPAREN,
                ")" => JassSymbol.RPAREN,
                "[" => JassSymbol.LBRACK,
                "]" => JassSymbol.RBRACK,
                _ => JassSymbol.NONE,
            };


            return result;
        }
    }
}
