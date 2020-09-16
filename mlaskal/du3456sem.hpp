/*

	DU3456SEM.H

	DB

	Mlaskal's semantic interface for DU3-6

*/

#ifndef __DU3456SEM_H
#define __DU3456SEM_H

#include <string>
#include "literal_storage.hpp"
#include "flat_icblock.hpp"
#include "dutables.hpp"
#include "abstract_instr.hpp"
#include "gen_ainstr.hpp"

namespace mlc {

	type_pointer get_pointer_type(symbol_tables* symbol, ls_id_index index, int line);

	unsigned long str_to_int(std::string word);
	bool out_of_range(unsigned long number);
	unsigned long trunc(unsigned long word);
	std::string uppercase(std::string word);


}

#endif
