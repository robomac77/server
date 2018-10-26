using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using NEO_Block_API.RPC;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;

namespace NEO_Block_API.lib
{
	public class mySqlHelper
	{
		public static string conf = "database=block;server=106.15.200.244;user id=root;Password=jingmian@mysql;sslmode=None";


		public JArray GetAddress(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				conn.Open();
				var addr = req.@params[0].ToString();

				string select = "select firstuse , lastuse , txcount from  address where addr = @addr";

				JsonPRCresponse res = new JsonPRCresponse();
				MySqlCommand cmd = new MySqlCommand(select, conn);
				cmd.Parameters.AddWithValue("@addr", addr);

				JArray bk = new JArray();
				MySqlDataReader rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{

					var adata = (rdr["firstuse"]).ToString();
					var ldata = (rdr["lastuse"]).ToString();
					var tdata = (rdr["txcount"]).ToString();

					 bk.Add(new JObject { { "firstuse", adata }, { "lastuse", ldata } , { "txcount", tdata } });


				}

					

				return res.result = bk;

			}
		}

		public JArray GetAddrs(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				conn.Open();
				

				string select = "select addr , firstdate , lastdate , firstuse , lastuse , txcount from address ";

				JsonPRCresponse res = new JsonPRCresponse();
				MySqlCommand cmd = new MySqlCommand(select, conn);
				

				MySqlDataReader rdr = cmd.ExecuteReader();

				JArray bk = new JArray();
				while (rdr.Read())
				{

					var adata = (rdr["addr"]).ToString();
					var fdata = (rdr["firstdate"]).ToString();
					var ldata = (rdr["lastdate"]).ToString();
					var f = (rdr["firstuse"]).ToString();
					var l = (rdr["lastuse"]).ToString();
					var txcount = (rdr["txcount"]).ToString();

					bk.Add(new JObject { { "addr", adata } , { "firstdate", fdata } , { "lastdate", ldata } , { "firstuse", f } , { "lastuse", l }, { "txcount", txcount } });
			
				}

				return res.result = bk;

			}
		}


		public JArray GetAddrsTxs(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				conn.Open();
				var addr = req.@params[0].ToString();

				string select = "select txid,addr from  address_tx where @addr = addr ";

				JsonPRCresponse res = new JsonPRCresponse();
				MySqlCommand cmd = new MySqlCommand(select, conn);
				cmd.Parameters.AddWithValue("@addr", addr);


				MySqlDataReader rdr = cmd.ExecuteReader();
				JArray bk = new JArray();
				while (rdr.Read())
				{

					var adata = (rdr["txid"]).ToString();
					var vdata = (rdr["addr"]).ToString();


					bk.Add(new JObject { { "txid", adata }, { "addr", vdata } });

				}

				return res.result = bk;

			}
		}

		public JArray GetTxCount(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				conn.Open();
		

				string select = "select count(*) from address_tx";

				JsonPRCresponse res = new JsonPRCresponse();
				MySqlCommand cmd = new MySqlCommand(select, conn);
				

				MySqlDataReader rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{

					var adata = (rdr["count(*)"]).ToString();
					
					

					JArray bk = new JArray {
					new JObject    {
										{"txcount",adata}
								   }
					

							   };

					res.result = bk;
				}

				return res.result;

			}
		}

		public JArray GetAddrCount(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				conn.Open();


				string select = "select count(*) from address";

				JsonPRCresponse res = new JsonPRCresponse();
				MySqlCommand cmd = new MySqlCommand(select, conn);



				MySqlDataReader rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{

					var adata = (rdr["count(*)"]).ToString();



					JArray bk = new JArray {
					new JObject    {
										{"addrcount",adata}
								   }


							   };

					res.result = bk;
				}

				return res.result;

			}
		}
		public JArray GetBalance(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				conn.Open();
				var address = req.@params[0].ToString();

				string select = "select address , balance from balance where address = @address";

				JsonPRCresponse res = new JsonPRCresponse();
				MySqlCommand cmd = new MySqlCommand(select, conn);
				cmd.Parameters.AddWithValue("@address", address);

				MySqlDataReader rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{

					var adata = (rdr["address"]).ToString();
					var ldata = (rdr["balance"]).ToString();
				

					JArray bk = new JArray {
					new JObject    {
										{"address",adata}
								   },
					new JObject    {
										{"addresss",ldata}
								   }
					

							   };

					res.result = bk;
				}

				return res.result;

			}
		}

		public JArray GetTx(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				conn.Open();
				string select = "select * from  address_tx ";
				MySqlDataAdapter adapter = new MySqlDataAdapter(select, conf);
				DataSet ds = new DataSet();
				adapter.Fill(ds);

				var data = ds.ToString();
				var alldata = Newtonsoft.Json.Linq.JArray.Parse(data);
				JsonPRCresponse res = new JsonPRCresponse();

				res.jsonrpc = req.jsonrpc;
				res.id = req.id;
				res.result = alldata;

				return alldata;

			}
		}

		public JArray GetAsset(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{


				conn.Open();
				var id = req.@params[0].ToString();

				string select = "select version , id , type , name , amount , available , precision , owner , admin, issuer , expiration , frozen  from  asset where id = @id";

				MySqlCommand cmd = new MySqlCommand(select, conn);
				cmd.Parameters.AddWithValue("@id", id);

				JsonPRCresponse res = new JsonPRCresponse();
				MySqlDataReader rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{

					var adata = (rdr["version"]).ToString();
					var idata = (rdr["id"]).ToString();
					var tdata = (rdr["type"]).ToString();
					var ndata = (rdr["name"]).ToString();
					var xdata = (rdr["amount"]).ToString();
					var mdata = (rdr["available"]).ToString();
					var pdata = (rdr["precision"]).ToString();
					var odata = (rdr["owner"]).ToString();
					var fdata = (rdr["admin"]).ToString();
					var qdata = (rdr["issuer"]).ToString();
					var rdata = (rdr["expiration"]).ToString();
					var wdata = (rdr["frozen"]).ToString();
					

					JArray bk = new JArray {
					new JObject    {
										{"version",adata}
								   },
					new JObject    {
										{"id",idata}
								   },
					new JObject    {
										{"type",tdata}
								   },
					new JObject    {
										{"name",ndata}
								   },
					new JObject    {
										{"amount",xdata}
								   },
					new JObject    {
										{"available",mdata}
								   },
					new JObject    {
										{"precision",pdata}
								   },
					new JObject    {
										{"owner",odata}
								   },
					new JObject    {
										{"admin",fdata}
								   },
					new JObject    {
										{"issuer",qdata}
								   },
					new JObject    {
										{"expiration",rdata}
								   },
					new JObject    {
										{"frozen",wdata}
					               }

							   };

					res.result = bk;
				}

				return res.result;

			}
		}

		public JArray GetAllAsset(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				

					conn.Open();
			

					string select = "select id , type , available , issuer from asset";

					JsonPRCresponse res = new JsonPRCresponse();
					MySqlCommand cmd = new MySqlCommand(select, conn);
				

					MySqlDataReader rdr = cmd.ExecuteReader();
					JArray bk = new JArray();
					while (rdr.Read())
					{

						var adata = (rdr["id"]).ToString();
					    var tdata = (rdr["type"]).ToString();
					    var xdata = (rdr["available"]).ToString();
					    var pdata = (rdr["issuer"]).ToString();



					bk.Add(new JObject { { "id", adata }, {"type", tdata } , { "available", xdata }, {"precision", pdata }});

					}

					return res.result = bk;

			}
		}

		public JArray GetBlock(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				JsonPRCresponse res = new JsonPRCresponse();
				conn.Open();

				var hash = req.@params[0].ToString();
				string select = "select size , version , previousblockhash , merkleroot , time , nonce , nextconsensus , script  from block where hash = @hash";

				MySqlCommand cmd = new MySqlCommand(select, conn);
				cmd.Parameters.AddWithValue("@hash", hash);


				MySqlDataReader rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{


					var sdata = (rdr["size"]).ToString();
					var adata = (rdr["version"]).ToString();
					var pdata = (rdr["previousblockhash"]).ToString();
					var mdata = (rdr["merkleroot"]).ToString();
					var tdata = (rdr["time"]).ToString();
					var ndata = (rdr["nonce"]).ToString();
					var nc = (rdr["nextconsensus"]).ToString();

					JArray bk = new JArray {
					new JObject    {
										{"size",sdata}
								   } ,
					new JObject    {
										{"version",adata}
								   },
					new JObject    {
										{"previoushash",pdata}
								   },
					new JObject    {
										{"merkleroot",mdata}
								   },
					new JObject    {
										{"time",tdata}
								   },
					new JObject    {
										{"nonce",ndata}
								   },
					new JObject    {
										{"nextconsensus",nc}
								   }
							   };

					res.result = bk;
				}

				return res.result;



			}
		}


		public JArray GetBlocks(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				JsonPRCresponse res = new JsonPRCresponse();
				conn.Open();

			
				string select = "select  size ,  version , previousblockhash , merkleroot , time , nonce , nextconsensus , script  from block  limit 10";

				MySqlCommand cmd = new MySqlCommand(select, conn);
				

				MySqlDataReader rdr = cmd.ExecuteReader();

				JArray bk = new JArray();

				while (rdr.Read())
				{

					var sdata = (rdr["size"]).ToString();
					var adata = (rdr["version"]).ToString();
					var pdata = (rdr["previousblockhash"]).ToString();
					var mdata = (rdr["merkleroot"]).ToString();
					var tdata = (rdr["time"]).ToString();
					var ndata = (rdr["nonce"]).ToString();
					var nc = (rdr["nextconsensus"]).ToString();
					var s = (rdr["script"]).ToString();

		

					bk.Add(new JObject { { "size", sdata }, { "version", adata }, { "previousblockhash", pdata }, { "merkleroot", mdata }, { "time", tdata } , { "nonce", ndata } , { "nextconsensus", nc } , { "script", s } });
				}

				return res.result = bk;



			}
		}

		public JArray GetNep5Asset(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{


					conn.Open();
					var assetid = req.@params[0].ToString();

					string select = "select totalsupply , name , symbol , decimals from nep5asset where assetid = @assetid";

					JsonPRCresponse res = new JsonPRCresponse();
					MySqlCommand cmd = new MySqlCommand(select, conn);
					cmd.Parameters.AddWithValue("@assetid", assetid);


					MySqlDataReader rdr = cmd.ExecuteReader();
					JArray bk = new JArray();
					while (rdr.Read())
					{

						var adata = (rdr["totalsupply"]).ToString();
						var ndata = (rdr["name"]).ToString();
						var sdata = (rdr["symbol"]).ToString();
						var ddata = (rdr["decimals"]).ToString();



						bk.Add(new JObject { { "totalsupply", adata }, { "name", ndata } , { "symbol", sdata } , { "decimals", ddata } });

					}

					return res.result = bk;

			}
		}

		public JArray GetAllNep5Assets(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				conn.Open();

				string select = "select assetid from nep5asset ";

				MySqlCommand cmd = new MySqlCommand(select, conn);
			
				JsonPRCresponse res = new JsonPRCresponse();

				MySqlDataReader rdr = cmd.ExecuteReader();
				JArray bk = new JArray();
				while (rdr.Read())
				{

					var adata = (rdr["assetid"]).ToString();
				


					bk.Add(new JObject { { "assetid", adata } });

				}
				return res.result = bk;

			}

		}

		public JArray GetNep5Transfer(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{

				conn.Open();
				var txid = req.@params[0].ToString();

				string select = "select blockindex, n , asset , from , to , value from nep5transfer where txid = @txid";

				MySqlCommand cmd = new MySqlCommand(select, conn);
				cmd.Parameters.AddWithValue("@txid", txid);


				JsonPRCresponse res = new JsonPRCresponse();

				MySqlDataReader rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{
					var bdata = (rdr["blockindex"]).ToString();
					var ndata = (rdr["n"]).ToString();
					var adata = (rdr["asset"]).ToString();
					var fdata = (rdr["from"]).ToString();
					var tdata = (rdr["to"]).ToString();
					var vdata = (rdr["value"]).ToString();

					JArray bk = new JArray {
					new JObject    {
										{"blockindex",bdata}
								   },
					new JObject    {
										{"n",ndata}
								   },
					new JObject    {
										{"asset",adata}
								   },
					new JObject    {
										{"from",fdata}
								   },
					new JObject    {
										{"to",tdata}
								   },
					new JObject    {
										{"value",vdata}
								   },


							   };

					res.result = bk;
				}

				return res.result;

			}
		}


		public JArray GetNep5TransferByAsset(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{

				conn.Open();
				var id = req.@params[0].ToString();

				string select = "select  asset , from , to , value from nep5transfer where id = @id";

				MySqlCommand cmd = new MySqlCommand(select, conn);
				cmd.Parameters.AddWithValue("@id", id);

				JsonPRCresponse res = new JsonPRCresponse();

		
				MySqlDataReader rdr = cmd.ExecuteReader();

				JArray bk = new JArray();
				while (rdr.Read())
				{
			
				
					var adata = (rdr["asset"]).ToString();
					var fdata = (rdr["from"]).ToString();
					var tdata = (rdr["to"]).ToString();
					var vdata = (rdr["value"]).ToString();


				   bk.Add(new JObject { { "asset", adata }, { "from ", fdata }, { "to", tdata }, { "value", vdata } });


			}

				return res.result = bk;

			}
		}

		public JArray GetNep5TransferByTxid(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{

				conn.Open();
				var txid = req.@params[0].ToString();

				string select = "select  id , asset , from , to , value from nep5transfer where txid = @txid";

				MySqlCommand cmd = new MySqlCommand(select, conn);
				cmd.Parameters.AddWithValue("@txid", txid);


				JsonPRCresponse res = new JsonPRCresponse();

				MySqlDataReader rdr = cmd.ExecuteReader();
				JArray bk = new JArray();
				while (rdr.Read())
				{

					var idata = (rdr["id"]).ToString();
					var adata = (rdr["asset"]).ToString();
					var fdata = (rdr["from"]).ToString();
					var tdata = (rdr["to"]).ToString();
					var vdata = (rdr["value"]).ToString();


					bk.Add(new JObject { { "id", idata }, { "asset ", adata }, { "from", fdata }, { "to", tdata } , { "value", vdata } });


				}

				return res.result = bk;

			}
		}
		public JArray GetAllNep5Transfers(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				conn.Open();
				string select = "select * from  nep5transfer";
				MySqlDataAdapter adapter = new MySqlDataAdapter(select, conf);
				DataSet ds = new DataSet();
				adapter.Fill(ds);
				var data = ds.ToString();
				var alldata = Newtonsoft.Json.Linq.JArray.Parse(data);
				JsonPRCresponse res = new JsonPRCresponse();
				res.jsonrpc = req.jsonrpc;
				res.id = req.id;
				res.result = alldata;

				return alldata;

			}
		}

		public JArray GetNotify(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				conn.Open();
				var txid = req.@params[0].ToString();

				string select = "select gasconsumed from notify where txid = @txid";

				MySqlCommand cmd = new MySqlCommand(select, conn);
				cmd.Parameters.AddWithValue("@txid", txid);

				JsonPRCresponse res = new JsonPRCresponse();

				MySqlDataReader rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{

					var adata = (rdr["gasconsumed"]).ToString();

					JArray bk = new JArray {
					new JObject    {
										{"gasconsumed",adata}
								   }

							   };

					res.result = bk;
				}

				return res.result;



			}
		}

		public JArray GetRawTransaction(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				conn.Open();

				var txid = req.@params[0].ToString();
				string select = "select vin , vout  from tx where txid = @txid";

				MySqlCommand cmd = new MySqlCommand(select, conn);
				cmd.Parameters.AddWithValue("@txid", txid);

				JsonPRCresponse res = new JsonPRCresponse();

				MySqlDataReader rdr = cmd.ExecuteReader();
				JArray bk = new JArray();
				while (rdr.Read())
				{

					var adata = (rdr["vin"]).ToString();
					var vdata = (rdr["vout"]).ToString();

					

					bk.Add(new JObject { { "vin ", adata }, { "vout ", vdata } });


				}

				return res.result = bk;


			}

		}

		public JArray GetRawTransactions(JsonRPCrequest req)  // needs a sorting by txtype miner , reg or issue
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				conn.Open();

				string select = "select txid , type , blockindex , size from tx limit 20";

				MySqlCommand cmd = new MySqlCommand(select, conn);
	

				JsonPRCresponse res = new JsonPRCresponse();

				MySqlDataReader rdr = cmd.ExecuteReader();

				JArray bk = new JArray();
				while (rdr.Read())
				{

					var adata = (rdr["txid"]).ToString();
					var vdata = (rdr["type"]).ToString();
					var bdata = (rdr["blockindex"]).ToString();
					var sdata = (rdr["size"]).ToString();

					bk.Add(new JObject { { "txid ", adata }, { "type ", vdata } , { "height ", bdata } , { "size ", sdata } });

				
				}

				return res.result = bk;


			}

		}



		public JArray GetUTXO(JsonRPCrequest req)
		{

			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				JsonPRCresponse res = new JsonPRCresponse();
				conn.Open();

				var txid = req.@params[0].ToString();
				string select = "select asset , value from utxo where txid = @txid";

				MySqlCommand cmd = new MySqlCommand(select, conn);
				cmd.Parameters.AddWithValue("@txid", txid);


				MySqlDataReader rdr = cmd.ExecuteReader();
				JArray bk = new JArray();

				while (rdr.Read())
				{

					var adata = (rdr["asset"]).ToString();

					var vdata = (rdr["value"]).ToString();


					bk.Add(new JObject { { "asset ", adata }, { "value ", vdata } });
				}

				return res.result = bk;
			}
		}


		public JArray GetBlockCount(JsonRPCrequest req)   // gets the last 1 in desc
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{
				conn.Open();

		
				string select = "select height from blockheight limit 1";

				MySqlCommand cmd = new MySqlCommand(select, conn);
		

				JsonPRCresponse res = new JsonPRCresponse();

				MySqlDataReader rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{
					
						var adata = (rdr["height"]).ToString();

						JArray bk = new JArray {
					 new JObject    {
										{"height",adata}
								   }

							   };

						res.result = bk;
					}
				

				return res.result;
			}


		}


		public JArray GetBlockTime(JsonRPCrequest req)
		{
			using (MySqlConnection conn = new MySqlConnection(conf))
			{

				JsonPRCresponse res = new JsonPRCresponse();
				conn.Open();

				var hash = req.@params[0].ToString();
				string select = "select size , version , previousblockhash , merkleroot , time , nonce , nextconsensus , script  from block where hash = @hash";

				MySqlCommand cmd = new MySqlCommand(select, conn);
				cmd.Parameters.AddWithValue("@hash", hash);


				MySqlDataReader rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{


					var sdata = (rdr["size"]).ToString();
					var adata = (rdr["version"]).ToString();
					var pdata = (rdr["previousblockhash"]).ToString();
					var mdata = (rdr["merkleroot"]).ToString();
					var tdata = (rdr["time"]).ToString();
					var ndata = (rdr["nonce"]).ToString();
					var nc = (rdr["nextconsensus"]).ToString();

					JArray bk = new JArray {
					new JObject    {
										{"size",sdata}
								   } ,
					new JObject    {
										{"version",adata}
								   },
					new JObject    {
										{"previoushash",pdata}
								   },
					new JObject    {
										{"merkleroot",mdata}
								   },
					new JObject    {
										{"time",tdata}
								   },
					new JObject    {
										{"nonce",ndata}
								   },
					new JObject    {
										{"nextconsensus",nc}
								   }
							   };

					res.result = bk;

				}

				return res.result;
			}

		}
	}

}




