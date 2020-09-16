<?php

/**
 * Class UpdateController
 */
class UpdateController extends Controller {
    protected $controller;

    /**
     * @param mixed $id
     * 
     * @return void
     */
    public function run($id) {

        $this->view = 'update';

        if( isset($_POST['id']) && isset($_POST['amount']) ) {
            $id = $_POST['id'];
            $amount = $_POST['amount'];
            DB::update_amount($id, $amount);
        } else {
            
        }
    }
}