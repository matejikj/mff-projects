<?php

/**
 * Class DeleteController
 */
class DeleteController extends Controller {
    protected $controller;

    /**
     * @param mixed $id
     * 
     * @return void
     */
    public function run($id) {

        $this->view = 'delete';

        if(isset($_GET['delete_id'])) {
            $id = $_GET['delete_id'];
            DB::delete_item($id);
        } else {
            
        }
    }
}