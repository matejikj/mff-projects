%{
	// this code is emitted into du3456l.cpp 
	// avoid macro redefinition warnings when compiling du1l.cpp
	#pragma warning (disable:4005)
	// avoid unreferenced parameter warnings when compiling du1l.cpp
	#pragma warning (disable:4100)
	// avoid unreferenced function warnings when compiling du1l.cpp
	#pragma warning (disable:4505)

	// allow access to YY_DECL macro
	#include "bisonflex.hpp"

	// allow access to context 
	// CHANGE THIS LINE TO #include "du3456g.hpp" WHEN THIS FILE IS COPIED TO du3456l.lex
	#include "du3456g.hpp"
	#include "du3456sem.hpp"

%}

/* DO NOT TOUCH THIS OPTIONS! */
%option noyywrap nounput batch noinput stack reentrant
%option never-interactive

WHITESPACE[ \r\t\f]
A[a|A]
B[b|B]
C[c|C]
D[d|D]
E[e|D]
F[f|F]
G[g|G]
H[h|H]
I[i|I]
J[j|J]
K[k|K]
L[l|L]
M[m|M]
N[n|N]
O[o|O]
P[p|P]
Q[q|Q]
R[r|R]
S[s|S]
T[t|T]
U[u|U]
V[v|V]
W[w|W]
X[x|X]
Y[y|Y]
Z[z|Z]
INTEGER[0-9]+
EXPONENT([eE][\+\-]*{INTEGER})
IDENTIFIER[a-zA-Z][a-zA-Z0-9]*
REAL([eE][+-]?[0-9]+)

%{
	typedef yy::mlaskal_parser parser;
	int bracketsCount;
	std::string newString;
%}

%x COMMENT
%x STRING

%%

\{ {
	bracketsCount = 1;
	BEGIN(COMMENT);
}

<COMMENT>{

\{ {
	bracketsCount++;
}

\n {
	ctx->curline++;
}

\} {
	bracketsCount--;

	if (bracketsCount == -1){
		(mlc::DUERR_UNEXPENDCMT, ctx->curline);
	}

	if ( bracketsCount == 0 ){
		BEGIN(INITIAL);

	}
}

<<EOF>> {
	message(mlc::DUERR_EOFINCMT, ctx->curline);
	BEGIN(INITIAL);
}

. {
	newString.append(yytext);
}

}


\' {
	newString = "";
	BEGIN(STRING);
}

<STRING>{

\'\' {
	newString.append("'");
}

\' {
	BEGIN(INITIAL);
	return parser::make_STRING(ctx->tab->ls_str().add(newString), ctx->curline);
}

. {
	newString.append(yytext);
}

\n {
	message(mlc::DUERR_EOLINSTRCHR, ctx->curline);
	ctx->curline++;
	BEGIN(INITIAL);
	return parser::make_STRING(ctx->tab->ls_str().add(newString), (ctx->curline - 1 ));
}

<<EOF>> {
	message(mlc::DUERR_EOFINSTRCHR, ctx->curline);
	BEGIN(INITIAL);
	return parser::make_STRING(ctx->tab->ls_str().add(newString), ctx->curline);
}

}

{INTEGER}{IDENTIFIER}? {
	unsigned long number = mlc::str_to_int(yytext);
	std::string stringNumber = std::to_string(number);

	if ( stringNumber.length() < strlen(yytext) ){
		message(mlc::DUERR_BADINT, ctx->curline, yytext);
	}

	if ( mlc::out_of_range(number) ){
		message(mlc::DUERR_INTOUTRANGE, ctx->curline, yytext);
		number = mlc::trunc(number);
	}

	return parser::make_UINT(ctx->tab->ls_int().add(number), ctx->curline);
}

{INTEGER}((\.{INTEGER}{REAL}?)|{REAL}){IDENTIFIER}? {
	double real = std::stod(yytext);
	std::string realStr = std::to_string(real);
	if (strlen(yytext) > realStr.length()) {
		message(mlc::DUERR_BADREAL, ctx->curline, yytext);
	}
	return parser::make_REAL(ctx->tab->ls_real().add(real), ctx->curline);
}

{P}{R}{O}{G}{R}{A}{M} {
	return parser::make_PROGRAM(ctx->curline);
}

{B}{E}{G}{I}{N}	{
	return parser::make_BEGIN(ctx->curline);
}

{E}{N}{D} {
	return parser::make_END(ctx->curline);
}

{L}{A}{B}{E}{L}	{
	return parser::make_LABEL(ctx->curline);
}

{C}{O}{N}{S}{T} {
	return parser::make_CONST(ctx->curline);
}

{V}{A}{R} {
	return parser::make_VAR(ctx->curline);
}

{T}{Y}{P}{E} {
	return parser::make_TYPE(ctx->curline);
}

{A}{R}{R}{A}{Y} {
	return parser::make_ARRAY(ctx->curline);
}

{R}{E}{C}{O}{R}{D} {
	return parser::make_RECORD(ctx->curline);
}

{P}{R}{O}{C}{E}{D}{U}{R}{E} {
	return parser::make_PROCEDURE(ctx->curline);
}

{F}{U}{N}{C}{T}{I}{O}{N} {
	return parser::make_FUNCTION(ctx->curline);
}

{G}{O}{T}{O} {
	return parser::make_GOTO(ctx->curline);
}

{I}{F} {
	return parser::make_IF(ctx->curline);
}

{O}{F} {
	return parser::make_OF(ctx->curline);
}

{T}{H}{E}{N} {
	return parser::make_THEN(ctx->curline);
}

{E}{L}{S}{E} {
	return parser::make_ELSE(ctx->curline);
}

{R}{E}{P}{E}{A}{T} {
	return parser::make_REPEAT(ctx->curline);
}

{U}{N}{T}{I}{L} {
	return parser::make_UNTIL(ctx->curline);
}

{W}{H}{I}{L}{E} {
	return parser::make_WHILE(ctx->curline);
}

{D}{O} {
	return parser::make_DO(ctx->curline);
}

{F}{O}{R} {
	return parser::make_FOR(ctx->curline);
}

{D}{I}{V} {
	return parser::make_OPER_MUL(mlc::DUTOKGE_OPER_MUL::DUTOKGE_DIV, ctx->curline);
}

{M}{O}{D} {
	return parser::make_OPER_MUL(mlc::DUTOKGE_OPER_MUL::DUTOKGE_MOD, ctx->curline);
}

{T}{O} {
	return parser::make_FOR_DIRECTION(mlc::DUTOKGE_FOR_DIRECTION::DUTOKGE_TO, ctx->curline);
}

{D}{O}{W}{N}{T}{O} {
	return parser::make_FOR_DIRECTION(mlc::DUTOKGE_FOR_DIRECTION::DUTOKGE_DOWNTO, ctx->curline);
}

{O}{R} {
	return parser::make_OR(ctx->curline);
}

{N}{O}{T} {
	return parser::make_NOT(ctx->curline);
}

\} {
	message(mlc::DUERR_UNEXPENDCMT, ctx->curline);
}

; {
	return parser::make_SEMICOLON(ctx->curline);
}
							
, {
	return parser::make_COMMA(ctx->curline);
}
							
\. {
	return parser::make_DOT(ctx->curline);
}

\+	{
	return parser::make_OPER_SIGNADD(mlc::DUTOKGE_OPER_SIGNADD::DUTOKGE_PLUS, ctx->curline);
}

\-	{
	return parser::make_OPER_SIGNADD(mlc::DUTOKGE_OPER_SIGNADD::DUTOKGE_MINUS, ctx->curline);
}

\*	{
	return parser::make_OPER_MUL(mlc::DUTOKGE_OPER_MUL::DUTOKGE_ASTERISK, ctx->curline);
}

\/	{
	return parser::make_OPER_MUL(mlc::DUTOKGE_OPER_MUL::DUTOKGE_SOLIDUS, ctx->curline);
}

= {
	return parser::make_EQ(ctx->curline);
}	
							
: {
	return parser::make_COLON(ctx->curline);
}	
							
\( {
	return parser::make_LPAR(ctx->curline);
}	
							
\) {
	return parser::make_RPAR(ctx->curline);
}	
						
\.\. {
	return parser::make_DOTDOT(ctx->curline);
}	
							
\[ {
	return parser::make_LSBRA(ctx->curline);
}	
							
\] {
	return parser::make_RSBRA(ctx->curline);
}	

:= {
	return parser::make_ASSIGN(ctx->curline);
}	

\<	{
	return parser::make_OPER_REL(mlc::DUTOKGE_OPER_REL::DUTOKGE_LT, ctx->curline);
}
			
\>	{
	return parser::make_OPER_REL(mlc::DUTOKGE_OPER_REL::DUTOKGE_GT, ctx->curline);
}
			
\<=	{
	return parser::make_OPER_REL(mlc::DUTOKGE_OPER_REL::DUTOKGE_LE, ctx->curline);
}
			
\>=	{
	return parser::make_OPER_REL(mlc::DUTOKGE_OPER_REL::DUTOKGE_GE, ctx->curline);
}
			
\<\> {
	return parser::make_OPER_REL(mlc::DUTOKGE_OPER_REL::DUTOKGE_NE, ctx->curline);
}

{IDENTIFIER} {
	return parser::make_IDENTIFIER(ctx->tab->ls_id().add(mlc::uppercase(yytext)), ctx->curline);
}

{WHITESPACE}+		/* go out with whitespaces */

\n			ctx->curline++;

.			message(mlc::DUERR_UNKCHAR, ctx->curline, *yytext, *yytext);

<<EOF>>		return parser::make_EOF(ctx->curline);

%%

namespace mlc {

	yyscan_t2 lexer_init(FILE * iff)
	{
		yyscan_t2 scanner;
		yylex_init(&scanner);
		yyset_in(iff, scanner);
		return scanner;
	}

	void lexer_shutdown(yyscan_t2 scanner)
	{
		yyset_in(nullptr, scanner);
		yylex_destroy(scanner);
	}

}