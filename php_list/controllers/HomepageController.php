<?php

/**
 * Class HomepageController
 */
class HomepageController extends Controller {

    /**
     * @param mixed $parameters
     * 
     * @return void
     */
    public function run($parameters) {

        $this->header = array(
            'title' => 'Homepage',
            'keywords' => ', shop',
            'description' => 'Your best eshop'
        );
        
        $this->view = 'homepage';
        
    }

}