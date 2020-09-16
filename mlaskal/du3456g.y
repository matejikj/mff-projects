%language "c++"
%require "3.0.4"
%defines
%define parser_class_name{ mlaskal_parser }
%define api.token.constructor
%define api.token.prefix{DUTOK_}
%define api.value.type variant
%define parse.assert
%define parse.error verbose

%locations
%define api.location.type{ unsigned }

%code requires
{
	// this code is emitted to du3456g.hpp

	// allow references to semantic types in %type
#include "dutables.hpp"

	// avoid no-case warnings when compiling du3g.hpp
#pragma warning (disable:4065)

// adjust YYLLOC_DEFAULT macro for our api.location.type
#define YYLLOC_DEFAULT(res,rhs,N)	(res = (N)?YYRHSLOC(rhs, 1):YYRHSLOC(rhs, 0))
// supply missing YY_NULL in bfexpg.hpp
#define YY_NULL	0
#define YY_NULLPTR	0
}

%param{ mlc::yyscan_t2 yyscanner }	// formal name "yyscanner" is enforced by flex
%param{ mlc::MlaskalCtx* ctx }

%start mlaskal

%code
{
	// this code is emitted to du3456g.cpp

	// declare yylex here 
	#include "bisonflex.hpp"
	YY_DECL;

	// allow access to context 
	#include "dutables.hpp"

	// other user-required contents
	#include <assert.h>
	#include <stdlib.h>

	#include "du3456sem.hpp"

    /* local stuff */
    using namespace mlc;

}

%token EOF	0	"end of file"

%token PROGRAM			/* program */
%token LABEL			    /* label */
%token CONST			    /* const */
%token TYPE			    /* type */
%token VAR			    /* var */
%token BEGIN			    /* begin */
%token END			    /* end */
%token PROCEDURE			/* procedure */
%token FUNCTION			/* function */
%token ARRAY			    /* array */
%token OF				    /* of */
%token GOTO			    /* goto */
%token IF				    /* if */
%token THEN			    /* then */
%token ELSE			    /* else */
%token WHILE			    /* while */
%token DO				    /* do */
%token REPEAT			    /* repeat */
%token UNTIL			    /* until */
%token FOR			    /* for */
%token OR				    /* or */
%token NOT			    /* not */
%token RECORD			    /* record */

/* literals */
%token<mlc::ls_id_index> IDENTIFIER			/* identifier */
%token<mlc::ls_real_index> REAL			    /* real number */
%token<mlc::ls_int_index> UINT			    /* unsigned integer */
%token<mlc::ls_str_index> STRING			    /* string */

/* delimiters */
%token SEMICOLON			/* ; */
%token DOT			    /* . */
%token COMMA			    /* , */
%token EQ				    /* = */
%token COLON			    /* : */
%token LPAR			    /* ( */
%token RPAR			    /* ) */
%token DOTDOT			    /* .. */
%token LSBRA			    /* [ */
%token RSBRA			    /* ] */
%token ASSIGN			    /* := */

/* grouped operators and keywords */
%token<mlc::DUTOKGE_OPER_REL> OPER_REL			    /* <, <=, <>, >=, > */
%token<mlc::DUTOKGE_OPER_SIGNADD> OPER_SIGNADD		    /* +, - */
%token<mlc::DUTOKGE_OPER_MUL> OPER_MUL			    /* *, /, div, mod, and */
%token<mlc::DUTOKGE_FOR_DIRECTION> FOR_DIRECTION		    /* to, downto */

%type<std::vector<mlc::ls_id_index>> identifier_list
%type<mlc::type_pointer> type
%type<mlc::field_list_ptr> record
%type<std::vector<mlc::ls_id_index>> field_list_vars

%type<mlc::ls_id_index> procedure_header
%type<mlc::ls_id_index> function_header
%type<mlc::ls_id_index> block_p_header
%type<mlc::parameter_list_ptr> formal_parameters
%type<mlc::parameter_list_ptr> formal_parameter
%type<mlc::parameter_list_ptr> header_params
%type<mlc::ls_id_index> block_p_def

%%

mlaskal: 
PROGRAM IDENTIFIER SEMICOLON block_p DOT;

block_p:
block_label block_const block_type block_var block_p_defs BEGIN statement statements END;

block:
block_label block_const block_type block_var BEGIN statement statements END;

block_label:
%empty
| LABEL uint_numbers SEMICOLON;

uint_numbers:
UINT {
	ctx->tab->add_label_entry(@1, $1, ctx->tab->new_label());
}
| uint_numbers COMMA UINT {
	ctx->tab->add_label_entry(@3, $3, ctx->tab->new_label());
};

block_const:
%empty
| CONST block_const_list;

block_const_list:
%empty
| constant
| constant SEMICOLON block_const_list;

constant: string_identifier	
| uint_identifier
| ureal_identifier
| ident_identifier
| int_identifier
| real_identifier;

block_type:
%empty
| TYPE block_type_list;

block_type_list:
IDENTIFIER EQ type SEMICOLON {
	ctx->tab->add_type(@1, $1, $3);
}
| block_type_list IDENTIFIER EQ type SEMICOLON {
	ctx->tab->add_type(@2, $2, $4);
};

block_var:
%empty
| VAR block_var_list;

block_var_fields:
identifier_list COLON type	{
	for(auto identifier: $1) {
	ctx->tab->add_var(@1, identifier, $3);
}}

block_var_list:
%empty
| block_var_fields SEMICOLON block_var_list
| block_var_fields;

identifier_list:
IDENTIFIER {
	$$.push_back($1);
}
| identifier_list COMMA IDENTIFIER {
	auto idents = $1;
	idents.push_back($3);
	$$ = idents;
};


block_p_defs:
%empty
| block_p_def block_p_defs;

block_p_def:
block_p_header SEMICOLON {
	ctx->tab->enter(@2, $1);
} block {
	ctx->tab->leave(@4);
} SEMICOLON;

block_p_header:
procedure_header {
	$$ = $1;
}
| function_header {
	$$ = $1;
};

procedure_header:
PROCEDURE IDENTIFIER header_params {
	ctx->tab->add_proc(@1, $2, $3);
	$$ = $2;
};

function_header:
FUNCTION IDENTIFIER header_params COLON IDENTIFIER{
	ctx->tab->add_fnc(@1, $2, mlc::get_pointer_type(ctx->tab, $5, @5), $3);
	$$ = $2;
};


/*

block_p_defs:
%empty
| block_p_def block_p_defs;

block_p_def:
proc_func_header SEMICOLON { ctx->tab->enter(@2, $1); } block { ctx->tab->leave(@4); } SEMICOLON;


*/

header_params:
%empty {
	$$ = mlc::create_parameter_list();
}
| LPAR formal_parameters RPAR {
	$$ = $2;
};

formal_parameters:
formal_parameter {
	$$ = $1;
}
| formal_parameter SEMICOLON formal_parameters {
	auto idents = mlc::create_parameter_list();
	idents->append_and_kill($1);
	idents->append_and_kill($3);
	$$ = idents;
};

formal_parameter:
VAR identifier_list COLON IDENTIFIER {
	auto idents = mlc::create_parameter_list();
	auto pt = mlc::get_pointer_type(ctx->tab, $4, @4);
	for(auto index : $2) {
		idents->append_parameter_by_reference(index, pt);
	}
	$$ = idents;
}
| identifier_list COLON IDENTIFIER {
	auto idents = mlc::create_parameter_list();
	auto pt = mlc::get_pointer_type(ctx->tab, $3, @3);
	for(auto index : $1) {
		idents->append_parameter_by_value(index, pt);
	}
	$$ = idents;	
};


statement:
m_statement
| u_statement;

statements:
%empty
| SEMICOLON statement statements;

m_statement:
statement_header m_statement_block;

m_statements:
%empty
| SEMICOLON m_statement m_statements;

u_statement:
statement_header u_statement_block;

u_statements:
%empty
| SEMICOLON u_statement u_statements;

statement_header:
%empty
| UINT COLON;

m_statement_block:
%empty
| IDENTIFIER ASSIGN expression
| IDENTIFIER function_call
| GOTO UINT
| BEGIN m_statement m_statements END
| IF expression THEN m_statement ELSE m_statement
| WHILE expression DO m_statement
| REPEAT m_statement m_statements UNTIL expression
| FOR IDENTIFIER ASSIGN expression FOR_DIRECTION expression DO m_statement
| IDENTIFIER DOT IDENTIFIER ASSIGN expression;

u_statement_block:
BEGIN u_statement u_statements END
| IF expression THEN statement
| IF expression THEN m_statement ELSE u_statement
| WHILE expression DO u_statement
| REPEAT u_statement u_statements UNTIL expression
| FOR IDENTIFIER ASSIGN expression FOR_DIRECTION expression DO u_statement;

expression:
simple_expression
| simple_expression OPER_REL simple_expression
| simple_expression EQ simple_expression;

simple_expression:
term simple_expression_cycle
| OPER_SIGNADD term simple_expression_cycle;

simple_expression_cycle:
%empty
| OPER_SIGNADD term simple_expression_cycle
| OR simple_expression_cycle;

term:
factor
| factor OPER_MUL term;


real_parameters:
expression
| expression COMMA real_parameters;

function_call:
%empty
| LPAR real_parameters RPAR;

factor: UINT
| REAL
| STRING
| IDENTIFIER
| IDENTIFIER LPAR real_parameters RPAR
| LPAR expression RPAR
| NOT factor;

/*

CONSTANTS

*/


ident_identifier:
IDENTIFIER EQ IDENTIFIER {

	auto symbol = ctx->tab->find_symbol($3);
	mlc::symbol_pointer sp = ctx->tab->find_symbol($3);

	if ( sp->kind() != SKIND_CONST )
	{
		message( DUERR_NOTCONST, @3, * $3);
	}
	
	if ( sp->kind() != SKIND_CONST )
	{
		message( DUERR_NOTCONST, @3, * $3);
	}
	
	if ( sp->access_const()->type()->cat() == TCAT_INT ) {
		mlc::ls_int_index oldintvalue = sp->access_const()->access_int_const()->int_value();
		mlc::ls_int_index intvalue = ctx->tab->ls_int().add(*oldintvalue);
		ctx->tab->add_const_int( @1, $1, intvalue);
	} else {
		if (sp->access_const()->type()->cat() == TCAT_STR) {
			mlc::ls_str_index oldstrvalue = sp->access_const()->access_str_const()->str_value();
			mlc::ls_str_index strvalue = ctx->tab->ls_str().add(*oldstrvalue);
			ctx->tab->add_const_str( @1, $1, strvalue);
		} else {
			if ( sp->access_const()->type()->cat() == TCAT_REAL ) {
				mlc::ls_real_index oldrealvalue = sp->access_const()->access_real_const()->real_value();
				mlc::ls_real_index realvalue = ctx->tab->ls_real().add(*oldrealvalue);
				ctx->tab->add_const_real( @1, $1, realvalue);
			} else {
				bool value = sp->access_const()->access_bool_const()->bool_value();
				ctx->tab->add_const_bool( @1, $1, value);
			}
		}
	}
};

uint_identifier:
IDENTIFIER EQ UINT {
	ctx->tab->add_const_int(@1, $1, $3);
};

int_identifier:
IDENTIFIER EQ OPER_SIGNADD UINT {
	if($3 == mlc::DUTOKGE_OPER_SIGNADD::DUTOKGE_MINUS)
	{
		mlc::ls_int_index minusvalue = ctx->tab->ls_int().add(- *$4);
		ctx->tab->add_const_int(@1, $1, minusvalue);
	}
	else
	{
		ctx->tab->add_const_int(@1, $1, $4);
	}
};

ureal_identifier:
IDENTIFIER EQ REAL {
	ctx->tab->add_const_real(@1, $1, $3);
};

real_identifier:
IDENTIFIER EQ OPER_SIGNADD REAL {
	if($3 == mlc::DUTOKGE_OPER_SIGNADD::DUTOKGE_MINUS)
	{
		mlc::ls_real_index minusvalue = ctx->tab->ls_real().add(- *$4);
		ctx->tab->add_const_real(@1, $1, minusvalue);
	}
	else
	{				
		ctx->tab->add_const_real(@1, $1, $4);
	}
};

string_identifier:
IDENTIFIER EQ STRING {
	ctx->tab->add_const_str(@1, $1, $3);
};

/*

TYPES

*/

type:
IDENTIFIER {
	$$ = mlc::get_pointer_type(ctx->tab, $1, @1);
}
| RECORD record END {
	$$ = ctx->tab->create_record_type($2, @1);
};

record:
field_list_vars COLON type SEMICOLON record {
	auto list = mlc::create_field_list();
	for (auto id: $1) {
	list->append_field(id, $3);
	}
	list->append_and_kill($5);
	$$ = list;
}
| %empty	{
	$$ = mlc::create_field_list();
};

field_list_vars:	
IDENTIFIER {
	$$.push_back($1);
}
| field_list_vars COMMA IDENTIFIER {
	auto ids = $1;
	ids.push_back($3);
	$$ = ids;
};

%%

namespace yy {

	void mlaskal_parser::error(const location_type& l, const std::string& m)
	{
		message(DUERR_SYNTAX, l, m);
	}

}

