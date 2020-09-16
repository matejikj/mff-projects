<?php

/**
 * Class DB
 */
class DB {

    /**
     * @var undefined
     */
    private static $conn;
    
    /**
     * @param array $config
     * 
     * @return void
     */
    public static function connectDB( $config = array() ){
        
        if (!isset(self::$conn)) {
            $server = isset($config['server'])? $config['server'] : 'localhost';
            $login = isset($config['login'])? $config['login'] : 'root';
            $password = isset($config['password'])? $config['password'] : '';
            $database = isset($config['database'])? $config['database'] : '';
            self::$conn = mysqli_connect($server, $login, $password, $database);
    
            /* check connection */
            if (mysqli_connect_errno()) {
                printf("Connect failed: %s\n", mysqli_connect_error());
                exit();
            }
        }
    }

    /**
     * @param mixed $sql
     * 
     * @return void
     */
    public static function query($sql){
        $result = [];
        if ($query = mysqli_query(self::$conn, $sql)) {
            while ($row = mysqli_fetch_assoc($query)) {
                array_push($result, $row);
            }
        }
        return $result;
    }

    /**
     * @return void
     */
    public static function get_items(){
        $sql = "SELECT l.id, i.name, l.amount, l.position FROM list l INNER JOIN items i ON l.item_id = i.id";
        $result = [];
        if ($query = mysqli_query(self::$conn, $sql)) {
            while ($row = mysqli_fetch_assoc($query)) {
                array_push($result, $row);
            }
        }
        return $result;
    }

    /**
     * @param mixed $id
     * @param mixed $amount
     * 
     * @return void
     */
    public static function update_amount($id,$amount) {
        $sql = "UPDATE list SET amount = " . $amount . " WHERE id = " . $id;
        mysqli_query(self::$conn, $sql);
    }

    /**
     * @param mixed $first_id
     * @param mixed $second_id
     * 
     * @return void
     */
    public static function do_swap($first_id, $second_id) {
        $sql = "UPDATE list SET position = position + 1 WHERE id = " . $first_id;
        mysqli_query(self::$conn, $sql);
        $sql = "UPDATE list SET position = position - 1 WHERE id = " . $second_id;
        mysqli_query(self::$conn, $sql);
    }

    /**
     * @return void
     */
    public static function get_names(){
        $sql = "SELECT name FROM items";
        $result = [];
        if ($query = mysqli_query(self::$conn, $sql)) {
            while ($row = mysqli_fetch_assoc($query)) {
                array_push($result, $row['name']);
            }
        }
        return $result;
    }

    /**
     * @param mixed $name
     * 
     * @return void
     */
    public static function add_name($name) {
        $name = mysqli_real_escape_string(self::$conn, $name);
        if (mysqli_query(self::$conn, "INSERT into items (name) VALUES ('$name')")) {
        }
    }

    /**
     * @param mixed $id
     * 
     * @return void
     */
    public static function delete_item($id){
        $position = DB::get_item_position($id);

        $sql = "UPDATE list SET position = position - 1 WHERE position > " . $position;
        mysqli_query(self::$conn, $sql);
        $sql = "DELETE FROM list WHERE id = " . $id;
        mysqli_query(self::$conn, $sql);


    }

    /**
     * @param mixed $name
     * @param mixed $amount
     * 
     * @return void
     */
    public static function add_item($name, $amount) {

        $name = mysqli_real_escape_string(self::$conn, $name);
        $amount = mysqli_real_escape_string(self::$conn, $amount);
        $item_id = DB::get_item_id($name);
        $count = DB::get_list_count();
        $count++;
        $sql = "INSERT INTO `list`(`item_id`, `amount`, `position`) VALUES (" . $item_id . "," . $amount . "," . $count . ")";
        mysqli_query(self::$conn, $sql);
    }

    /**
     * @param mixed $name
     * 
     * @return void
     */
    public static function get_item_id($name) {
        $sql = "SELECT `id` FROM `items` WHERE `name` = '$name'";
        $item_id;
        if ($query = mysqli_query(self::$conn, $sql)) {
            while ($row = mysqli_fetch_assoc($query)) {
                $item_id = $row['id'];
            }
        }
        return $item_id;
    }

    /**
     * @param mixed $id
     * 
     * @return void
     */
    public static function get_item_position($id) {
        $sql = "SELECT position FROM list WHERE id = " . $id;
        $position;
        if ($query = mysqli_query(self::$conn, $sql)) {
            while ($row = mysqli_fetch_assoc($query)) {
                $position = $row['position'];
            }
        }
        return $position;
    }


    /**
     * @return void
     */
    public static function get_list_count() {
        $sql = "SELECT MAX(position) AS lastPos FROM list";
        $count;
        if ($query = mysqli_query(self::$conn, $sql)) {
            while ($row = mysqli_fetch_assoc($query)) {
                $count = $row['lastPos'];
            }
        }
        return $count;
    }
}