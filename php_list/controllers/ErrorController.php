<?php

/**
 * Class ErrorController
 */
class ErrorController extends Controller {

    /**
     * @param mixed $parameters
     * 
     * @return void
     */
    public function run($parameters) {
        
        header("HTTP/1.0 404 Not Found");
        
        $this->header['title'] = 'Error 404';
        $this->header['description'] = 'Error 404';
        $this->header['keywords'] = 'Error 404';

        
        $this->view = 'error';
    }
}