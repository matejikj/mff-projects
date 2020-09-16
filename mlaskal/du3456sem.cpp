/*

DU3456SEM.CPP

JY

Mlaskal's semantic interface for DU3-6

*/

#include "du3456sem.hpp"
#include "duerr.hpp"

namespace mlc {
    unsigned long str_to_int(std::string word){
        unsigned long number = 0;

        int i = 0;

        int tmpChar;

        while (i < word.length()){

            if ( word[i]<='9' && word[i]>='0' ){
                tmpChar = word[i] - '0';
                number = number * 10;
                number = number + tmpChar;
            } else {
                return number;
            }
            
            i++;
        }

        return number;
    }

    bool out_of_range(unsigned long number){
        if ( number > 2147483647 ){
            return true;
        } else {
            return false;
        }
    }

    unsigned long trunc( unsigned long word ){
        return word & 0x000000007FFFFFFF;
    }

    std::string uppercase(std::string word ){
        std::string newString = "";
        for ( int i = 0 ; i < word.length() ; i++){
            newString += std::toupper(word[i]);
        }

        return newString;
    }

    type_pointer get_pointer_type(symbol_tables* symbol, ls_id_index index, int line) {
		auto res = symbol->find_symbol(index)->access_type();
		if (!res)
		{
			message(DUERR_NOTTYPE, line, *index);
		}
		return res->type();
	}


};

/*****************************************/