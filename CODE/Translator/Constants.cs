namespace CSharpToJavaTranslator
{
    public class Constants
    {
        public enum TokenType
        {
            //Не поддерживаются следующие ключевые и контекстные слова:
            //abstract, as, base, catch, checked, delegate,
            //event, explicit, extern, finally, fixed, goto,
            //implicit, interface, is, lock,
            //operator, out, override, params, readonly, ref,
            //sealed, sizeof, stackalloc, throw, try, typeof, 
            //unchecked, unsafe, virtual, volatile

            //add, and, alias, ascending, args, async, await, by,
            //descending, dynamic, equals, from, get, global,
            //group, init, into, join, let, managed, nameof,
            //nint, not, notnull, nuint, on, orderby, partial,
            //record, remove, select, set, unmanaged, value, var,
            //when, where, with, yield

            USING,
            NAMESPACE,
            IDENTIFIER,
            STRUCT,
            CLASS,
            ENUM,
            IF,
            ELSE,
            WHILE,
            DO,
            FOR,
            FOREACH,
            SWITCH,
            CASE,
            BREAK,
            CONTINUE,
            DEFAULT,
            NEW,
            RETURN,
            IN,

            PUBLIC,
            PROTECTED,
            PRIVATE,
            INTERNAL,
            CONST,
            STATIC,
            THIS,

            INT_NUMBER,
            REAL_NUMBER,
            STRING,
            CHAR,
            NULL,
            TRUE,
            FALSE,

            DOT, // .
            COMMA, // ,
            COLON, // :
            SEMICOLON, // ;
            QUESTION_MARK, // ?
            OPENING_CURLY_BRACKET, // {
            CLOSING_CURLY_BRACKET, // }
            OPENING_BRACKET, // (
            CLOSING_BRACKET, // )
            OPENING_SQUARE_BRACKET, // [
            CLOSING_SQUARE_BRACKET, // ]
            SLASH, // /
            QUOTATION_MARK, // '
            DOUBLE_QUOTATION_MARK, // "

            PLUS, // +
            MINUS, // -
            MULTIPLICATION, // *
            DIVISION, // /
            MODULO, // %
            UNARY_MINUS, // -
            INCREMENT, // ++
            DECREMENT, // --

            NOT, // !
            AND, // &&
            OR, // ||
            EQUAL, // ==
            NOT_EQUAL, // !=
            GREATER, // >
            LESS, // <
            GREATER_OR_EQUAL, // >=
            LESS_OR_EQUAL, // <=

            BIT_TILDA, // ~
            BIT_EXCLUSIVE_OR, // ^
            BIT_AND, // &
            BIT_OR, // |
            BIT_ARITHMETIC_SHIFT_TO_RIGHT, // >>
            BIT_ARITHMETIC_SHIFT_TO_LEFT, // <<
            BIT_SHIFT_TO_RIGHT, // >>>

            ASSIGNMENT, // =
            PLUS_ASSIGNMENT, // +=
            MINUS_ASSIGNMENT, // -=
            MULTIPLICATION_ASSIGNMENT, // *=
            DIVISION_ASSIGNMENT, // /=
            MODULO_ASSIGNMENT, // %=
            AND_ASSIGNMENT, // &=
            EXCLUSIVE_OR_ASSIGNMENT, // ^=
            OR_ASSIGNMENT, // |=
            ARITHMETIC_SHIFT_TO_RIGHT_ASSIGNMENT, // >>=
            ARITHMETIC_SHIFT_TO_LEFT_ASSIGNMENT, // <<=
            SHIFT_TO_RIGHT_ASSIGNMENT, // >>>=

            UTILITY_TERNARY_CLOSED_IF,
            UTILITY_FUNCTION, // имя функции
            UTILITY_ARRAY_ELEMENT_ACCESS, // []
            UNKNOWN
        }
        
        public enum State
        {
            EXPECTING_USING_OR_NAMESPACE,
            EXPECTING_IDENTIFIER,

            EXPECTING_SEMICOLON,
            EXPECTING_OPENING_CURLY_BRACKET,
            EXPECTING_CLOSING_CURLY_BRACKET,
            EXPECTING_OPENING_BRACKET,
            EXPECTING_CLOSING_BRACKET,
            EXPECTING_OPENING_SQUARE_BRACKET,
            EXPECTING_CLOSING_SQUARE_BRACKET,
            EXPECTING_DOT_OR_SEMICOLON,

            EXPECTING_EXPRESSION,
            EXPECTING_OPERATOR,
            EXPECTING_OPERAND,

            EXPECTING_CONTENT_OR_CLOSING_CURLY_BRACKET,
            EXPECTING_CONTENT_OR_OPENING_CURLY_BRACKET,
            EXPECTING_CONST_OR_STATIC_OR_STRUCT_OR_CLASS,
            EXPECTING_STATIC_OR_STRUCT_OR_CLASS,
            EXPECTING_STRUCT_OR_CLASS,
            EXPECTING_CLASS,
            EXPECTING_STATIC_OR_CONST_OR_CLASS_OR_STRUCT_OR_ENUM_OR_DATA_TYPE,
            EXPECTING_CLASS_OR_DATA_TYPE,
            EXPECTING_MEMBER_IDENTIFIER,
            EXPECTING_FIELD_IDENTIFIER,
            EXPECTING_DATA_TYPE,
            EXPECTING_SEMICOLON_OR_OPENING_BRACKET,
            EXPECTING_STATIC_OR_CLASS,
            EXPECTING_ENUM_CONTENT_OR_CLOSING_CURLY_BRACKET,
            EXPECTING_COMMA_OR_ASSIGNMENT_OR_CLOSING_CURLY_BRACKET,
            EXPECTING_MINUS_OR_INT_NUMBER,
            EXPECTING_INT_NUMBER,
            EXPECTING_COMMA_OR_CLOSING_CURLY_BRACKET,
            EXPECTING_NAME_OF_SELF,
            EXPECTING_ASSIGNMENT,
            EXPECTING_SEMICOLON_OR_OPENING_BRACKET_OR_EXPRESSION,
            EXPECTING_COMMA_OR_ASSIGNMENT_OR_CLOSING_BRACKET,
            EXPECTING_OPENING_SQUARE_BRACKET_OR_IDENTIFIER,
            EXPECTING_COMMA_OR_CLOSING_BRACKET,
            EXPECTING_WHILE,
            EXPECTING_COMMA_OR_SEMICOLON,
            EXPECTING_CLOSING_BRACKET_OR_COMMA,
            EXPECTING_IN,
            EXPECTING_DOT_OR_OPENING_SQUARE_BRACKET_OR_IDENTIFIER,
            EXPECTING_COMMA_OR_SEMICOLON_OR_IN,
            EXPECTING_IN_OR_DATA_TYPE,
            EXPECTING_ASSIGNMENT_OR_COMMA_OR_SEMICOLON,
            EXPECTING_ASSIGNMENT_OR_COMMA_OR_SEMICOLON_OR_IN
        }

        public enum TreeNodeType
        {
            PROGRAM_BEGINNING,

            USING,
            NAMESPACE,
            CLASS,
            STRUCT,
            ENUM,

            IF,
            ELSE,
            FOR,
            FOREACH,
            DO,
            WHILE,
            BREAK,
            CONTINUE,
            SWITCH,
            CASE,
            DEFAULT,
            RETURN,
            
            DECLARATION,
            METHOD,
            FIELD,
            EXPRESSION,
            EXPRESSION_RPN,
            MEMBER,
            PARAMETER
        }
    }
}