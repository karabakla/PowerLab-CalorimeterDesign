using System.Runtime.InteropServices;
using System.Text;

public class IEEE 
{
	[DllImport("ieee_32m.dll")]
	public static extern void ieee488_initialize (int myaddr, int level);
	[DllImport("ieee_32m.dll")]
	public static extern int  ieee488_transmit (string  cmd, int maxlen,out int  status);
	[DllImport("ieee_32m.dll")]
	public static extern int ieee488_receive (StringBuilder buf , int maxlen, out int len, out int  status);
	[DllImport("ieee_32m.dll")]
	public static extern int ieee488_send (int addr, string buf, int maxlen, out int  status);
	[DllImport("ieee_32m.dll")]
	public static extern int ieee488_enter (StringBuilder buf , int maxlen, out int len,int myaddr, out int  status);
	[DllImport("ieee_32m.dll")]
	public static extern int ieee488_spoll (int addr,out byte poll, out int  status);
	[DllImport("ieee_32m.dll")]
	public static extern void ieee488_ppoll (out byte poll);
	[DllImport("ieee_32m.dll")]
	public static extern int ieee488_tarray (out byte[] buf,int count,int eoi,out int  status);
	[DllImport("ieee_32m.dll")]
	public static extern int ieee488_rarray (out byte[] buf,int count, int len, out int  status);

	[DllImport("ieee_32m.dll")]
	public static extern char ieee488_srq();

	[DllImport("ieee_32m.dll")]
	public static extern void ieee488_setport (int bd, uint io);

	[DllImport("ieee_32m.dll")]
	public static extern void ieee488_boardselect (int bd);
	[DllImport("ieee_32m.dll")]
	public static extern void ieee488_dmachannel (int chan);
	[DllImport("ieee_32m.dll")]
	public static extern void ieee488_settimeout (uint timeout);
	[DllImport("ieee_32m.dll")]
	public static extern void ieee488_setoutputEOS (int e1,int e2);
	[DllImport("ieee_32m.dll")]
	public static extern void ieee488_setinputEOS (int eos_c);
	[DllImport("ieee_32m.dll")]
	public static extern void ieee488_enable_488ex(int e);
	[DllImport("ieee_32m.dll")]
	public static extern void ieee488_enable_488sd(int e, int t);
	[DllImport("ieee_32m.dll")]
	public static extern char ieee488_listener_present(int addr);
	[DllImport("ieee_32m.dll")]
	public static extern char ieee488_board_present();
	[DllImport("ieee_32m.dll")]
	public static extern int ieee488_feature (int f);
	[DllImport("ieee_32m.dll")]
	public static extern int ieee488_waitSRQDevice (int addr ,string pool , out int  status);

	public static void initialize(int addr, int level) 
	{ieee488_initialize(addr,level);}
	public static void transmit(string cmd, out int status)
	{ieee488_transmit(cmd, 0xFFFF, out status);}
	public static void receive(out string s, int maxlen, int len, out int status)
	{
		int l,st;
		StringBuilder sb = new StringBuilder( maxlen );
		ieee488_receive( sb, maxlen, out len, out status );
		s = sb.ToString();
	}
	public static void send(int addr, string s, out int status) 
	{ieee488_send(addr,s,0xffff, out status);}
	public static void enter(out string s, int maxlen, out int len, int addr, out int status) 
	{
		int st,l,retval;
		StringBuilder sb = new StringBuilder( maxlen );
		retval = ieee488_enter( sb, maxlen, out len, addr, out status );
		s = sb.ToString();
	}
	public static void spoll(int addr, out byte poll, out int status)
	{ieee488_spoll(addr, out poll, out status);}
	public static void ppoll(out byte poll)                    
	{ieee488_ppoll(out poll);}
	public static void tarray(out byte[] d, int count,int eoi, out int status)
	{ieee488_tarray(out d,count,eoi, out status);}
	public static void rarray(out byte[] d, int count, int len, out int status)
	{ieee488_rarray(out d,count,len, out status);}
	public static char srq()
	{return ieee488_srq();}
	public static void setport(int bd, uint io)
	{ieee488_setport(bd,io);}
	public static void boardselect(int bd)
	{ieee488_boardselect(bd);}
	public static void dmachannel(int c)
	{ieee488_dmachannel(c);}
	public static void settimeout(uint t)
	{ieee488_settimeout(t);}
	public static void setoutputEOS(int e1, int e2)
	{ieee488_setoutputEOS(e1,e2);}
	public static void setinputEOS(int e)
	{ieee488_setinputEOS(e);}
	public static void enable_488ex(int e)
	{ieee488_enable_488ex(e);}
	public static void enable_488sd(int e, int t)
	{ieee488_enable_488sd(e,t);}
	public static char listener_present(int a) 
	{return ieee488_listener_present(a);}
	public static char gpib_board_present()
	{return ieee488_board_present();}
	public static int gpib_feature(int f)     
	{return ieee488_feature(f);}
	public static int waitSRQDevice(int addr, string poll, out int status)
	{return ieee488_waitSRQDevice(addr,poll,out status);}
													   
}