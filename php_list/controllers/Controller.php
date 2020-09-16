<?php

/**
 * Class Controller
 */
abstract class Controller {

    /**
     * @var array
     */
    protected $data = array();

    /**
     * @var string
     */
    protected $view = "";

    /**
     * @var array
     */
    protected $header = array('title' => '', 'words' => '', 'description' => '', 'keywords' => '' );

    /**
     * @return void
     */
    public function showView() {
        if ( $this->view ) {
            extract($this->data);
            require("views/" . $this->view . ".phtml" );
        }
    }

    /**
     * @param mixed $url
     * 
     * @return void
     */
    public function route($url) {
        header("Location: /$url");
        header("Connection closed");
        exit;
    }

    /**
     * @param mixed $parameters
     * 
     * @return void
     */
    abstract function run($parameters);

}