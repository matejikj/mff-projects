<?php

class ListController extends Controller {

    public function run($parameters) {

        $this->header = array(
            'title' => 'Shopping cart',
            'keywords' => 'buy, items, list',
            'description' => 'Your shoping cart'
        );

        if (isset($_POST["first_id"]) && isset($_POST["second_id"]))
		{
            {
                $first_id = $_POST['first_id'];
                $second_id = $_POST['second_id'];
                DB::do_swap($first_id, $second_id);
			}
        }

        $names = DB::get_names();

        if (isset($_POST["name"]))
		{
            $name = $_POST["name"];
            if (!in_array($name, $names)){
                DB::add_name($name);
            }
            $amount;
			if (!isset($_POST['amount']))
			{
                $amount = "0";
            } else {
                $amount = $_POST['amount'];
            }
            DB::add_item($name, $amount);
        }

        $list_items = DB::get_items();

        usort($list_items,function($first,$second){
            return $first['position'] > $second['position'];
        });

        $this->data['names'] = $names;
        $this->data['items'] = $list_items;
        $this->view = 'list';
    }
}