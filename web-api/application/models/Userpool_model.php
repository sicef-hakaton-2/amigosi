<?php
defined('BASEPATH') OR exit('No direct script access allowed');

class Userpool_model extends CI_Model {
	public function __construct()
    {
            parent::__construct();
    }
     public function imeiExists($id)
        {
        	$id = $this->db->escape($id);
        	$q = $this->db->query("SELECT 1 FROM userpool WHERE imei = $id LIMIT 1;");
        	if($q->num_rows()) //broj istih viewida
        		return true;
        	return false;
        }
    public function getPool($imei)	//
    {
        $imei = $this->db->escape($imei);
		$q = $this->db->query("SELECT * FROM userpool WHERE imei = $imei LIMIT 1;");
		if(!$q->num_rows())
			return false;
		$row = $q->row_array();
		return $row;
    }
    public function setPool()
    {
    	if( isset($_POST['imei']) && !empty($_POST['imei']) && 
        	isset($_POST['location']) && !empty($_POST['location']) &&
        	isset($_POST['longitude']) &&
        	isset($_POST['lattitude'])
        	)
    	{
    			$now = strftime("%Y-%m-%d %H:%M:%S",time());

    			$imei 			= $this->db->escape_str($_POST['imei']);
    			if(isset($_POST['firstseen']) && !empty($_POST['firstseen']))
        			$firstseen 		= $this->db->escape_str($_POST['firstseen']);
        		else
        			$firstseen 		= $now;
        		$location 		= $this->db->escape_str($_POST['location']);
        		$longitude 		= $this->db->escape($_POST['longitude']);
        		$lattitude 		= $this->db->escape($_POST['lattitude']);
        		
        		if(!$this->imeiExists($imei))
	        		return false;
	        	//TODO PROVERI AUTH to je za unos u POOL treba se napravi Token_model
	        	//$q = $this->db->query("INSERT INTO userpool ")
    	}
        
    }
}