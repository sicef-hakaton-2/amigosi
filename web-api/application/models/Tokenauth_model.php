<?php
defined('BASEPATH') OR exit('No direct script access allowed');

class Tokenauth_model extends CI_Model {
	public function __construct()
    {
            parent::__construct();
    }
    public function isAuthorised()
    {
        if(!isset($_POST['token']) && empty($_POST['token']))
            return false;

        $id = $this->db->escape_str($_POST['token']);
        return $this->checkToken($id);

    }
    public function checkToken($id) //proveritoken
    {
        	$id = $this->db->escape($id);
        	$q = $this->db->query("SELECT 1 FROM tokens WHERE token = $id LIMIT 1;");
        	if($q->num_rows()) 
        		return true;
        	return false;
    }
    public function generateToken($description)	
    {
        $token = sha1(randomNumber(13));
        $description = $this->db->escape($_POST['description']);
        if(!isset($_POST['description']) && empty($_POST['description']))
            return false;
        $this->db->query("INSERT INTO token (id,token,description) VALUES (NULL,'{$token}','{$description}')"); //TODO: check
        return true;
    }
        
}