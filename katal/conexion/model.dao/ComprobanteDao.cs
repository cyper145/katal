using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using katal.conexion.model.entity;
using katal.Model;

namespace katal.conexion.model.dao
{
    public class ComprobanteDao : Obligatorio<Comprobante>
    {

        private Conexion objConexion;
        private SqlCommand comando;
        private string Bd;

        public ComprobanteDao(string bd)
        {
            Bd = bd;
            ApplicationUser user = AuthHelper.GetLoggedInUserInfo();

            if (user != null)
            {
                objConexion = Conexion.saberEstado(user.codEmpresa + "BDCOMUN");
            }
            
            ///0      
        }
        /*
    public void create(Comprobante obj)
        {
            DateTime date = DateTime.Now;
            string sEstado = "";
            decimal nPorcen = 0;
            decimal nValCIF = 0;
            string anios = date.Year.ToString("0000.##");
            string mes = date.Month.ToString("00.##");
            string msAnoMesProc = anios + mes;
            string verificacion = verificaDoc_cxc(obj.ANEX_CODIGO ,obj.TIPODOCU_CODIGO,obj.CSERIE , obj.CNUMERO);

            string cCorre = "";
            if (verificacion != "" && verificacion != "El Documento Esta en Cuentas x Pagar")
            {
               
                try
                {
                    string Cadenar = "UPDATE [014BDCOMUN].[dbo].COMPROBANTECAB SET CNUMORDCO='" + obj.CTDREFER.Trim() + "',";
                    Cadenar += " NUMRETRAC='" + obj.NUMRETRAC + "', CAOCOMPRA='" + obj.CAOCOMPRA + "',";
                    Cadenar += " dvence=" + obj.DVENCE + "";
                    Cadenar += ",FECRETRAC=" + obj.FECRETRAC;
                    Cadenar += " WHERE CAMESPROC='" + msAnoMesProc + "' AND CORDEN='" + obj.CORDEN + "'";
                    comando = new SqlCommand(Cadenar, objConexion.getCon());
                    objConexion.getCon().Open();
                    comando.ExecuteNonQuery();

                    string  CadenaD = "SELECT EDOC_OBLIGA FROM ESTADODOC WHERE EDOC_CLAVE = '1'";
                    comando = new SqlCommand(CadenaD, objConexion.getCon());
                    objConexion.getCon().Open();
                    SqlDataReader readinterno = comando.ExecuteReader();
                    if (readinterno.Read())
                    {
                        bool docobliga =Conversion.ParseBool( readinterno[0].ToString());
                        if (docobliga)
                        {
                            sEstado = "0";
                        }
                        else
                        {
                            sEstado = "1";
                        }
                    }
                    switch (obj.CDESTCOMP)
                    {
                        case "002":
                            nPorcen = 100;
                            nValCIF = 0;
                            break;
                        case "005":
                            //falta capturar esta data
                            nPorcen = 0;
                            nValCIF = 0;
                            break;
                      
                    }

                    cCorre = funcAutoNum(msAnoMesProc);


                    CadenaD = "INSERT INTO COMPROBANTECAB (";
                    CadenaD +=  "EMP_CODIGO,CORDEN,ANEX_CODIGO,ANEX_DESCRIPCION,TIPODOCU_CODIGO,CSERIE, ";
                    CadenaD += "CNUMERO, DEMISION, DVENCE, DRECEPCIO, TIPOMON_CODIGO, ";
                    CadenaD += "NIMPORTE, TIPOCAMBIO_VALOR, CDESCRIPC, RESPONSABLE_CODIGO, CESTADO, ";
                    CadenaD += "NSALDO, CCODCONTA, CFORMPAGO, CSERREFER, CNUMREFER, ";
                    CadenaD += "CTDREFER, CONVERSION_CODIGO, DREGISTRO, CTIPPROV, CNRORUC, ";
                    CadenaD += "ESTCOMPRA, CDESTCOMP, DIASPAGO, CIGVAPLIC, CCONCEPT, ";
                    CadenaD += "DFECREF, NTASAIGV, NIGV, NPORCE, CCODRUC, ";
                    CadenaD += "LHONOR, NIR4, NIES, NTOTRH, NBASEIMP, ";
                    CadenaD += "NVALCIF, DCONTAB, CAMESPROC, CSALDINI,NPERCEPCION,NUMRETRAC,";
                    CadenaD += "FECRETRAC,CNUMORDCO,";
                    CadenaD += "CO_L_RETE,LDETRACCION,NTASADETRACCION, DETRACCION,COD_SERVDETRACC,COD_TIPOOPERACION,";
                    CadenaD += "nImporteRef, RCO_TIPO, RCO_SERIE, RCO_NUMERO,";
                    // If IsDate(txtFecDocRef) Then
                    CadenaD += "RCO_FECHA,";
                    CadenaD += "flg_RNTNODOMICILIADO,CAOCOMPRA) VALUES (";
                    CadenaD += "'" + GridViewHelper.user.codEmpresa + "','" + cCorre + "', '" + obj.ANEX_CODIGO + "', '" + obj.ANEX_DESCRIPCION + "', '" + obj.TIPODOCU_CODIGO + "', '" + obj.CSERIE + "',";
                    CadenaD += "'" + obj.CNUMERO + "', " + obj.DEMISION + ", " + obj.DVENCE + ", " + obj.DRECEPCIO + ", '" + obj.TIPOMON_CODIGO + "',";
                    //End If
                    //
                    if(obj.CDESTCOMP.Trim()== "007" || obj.CDESTCOMP.Trim()=="008")
                    {
                       // CadenaD += "" + IIf(true, Math.Round(Val(obj.NTOTRH), 2) - Math.Round(Val(obj.NIR4), 2), Math.Round(Val(obj.NIMPORTE), 2)) + ", ";
                        CadenaD += "" + IIfData(true, Math.Round(obj.NTOTRH, 2) - Math.Round(obj.NIR4, 2), Math.Round(obj.NIMPORTE, 2)) + ", ";                                      
                    }
                    else
                        if(obj.CDESTCOMP.Trim() == "006")
                         {
                            CadenaD += "" + IIfData(true, Math.Round(obj.NTOTRH, 2) - Math.Round(obj.NIR4, 2) + Math.Round(obj.NIMPORTE, 2), Math.Round(obj.NIMPORTE, 2)) + ", ";
                         }
                        else
                        {
                             CadenaD += "" + IIfData(true, Math.Round(obj.NTOTRH, 2), Math.Round(obj.NIMPORTE, 2)) + ", ";                      
                        }

                 //   CadenaD += "" + obj.CONVERSION_CODIGO == "ESP" ? obj.TIPOCAMBIO_VALOR  : 0    + ", '" + obj.CDESCRIPC + "', '" + obj.RESPONSABLE_CODIGO + "', '" + sEstado + "', ";
                    CadenaD += "" + obj.CONVERSION_CODIGO    + ", '" + obj.CDESCRIPC + "', '" + obj.RESPONSABLE_CODIGO + "', '" + sEstado + "', ";
                    //
                    if (obj.CDESTCOMP.Trim() == "007" || obj.CDESTCOMP.Trim() == "008")
                    {
                        // CadenaD += "" + IIf(true, Math.Round(Val(obj.NTOTRH), 2) - Math.Round(Val(obj.NIR4), 2), Math.Round(Val(obj.NIMPORTE), 2)) + ", ";
                        CadenaD += "" + IIfData(true, Math.Round(obj.NTOTRH + obj.NPERCEPCION, 2) - Math.Round(obj.NIR4, 2), Math.Round(obj.NIMPORTE + obj.NPERCEPCION, 2)) + ", ";
                    }
                    else
                        if (obj.CDESTCOMP.Trim() == "006")
                    {
                        CadenaD += "" + IIfData(true, Math.Round(obj.NTOTRH + obj.NPERCEPCION, 2) - Math.Round(obj.NIR4, 2) + Math.Round(obj.NIES, 2), Math.Round(obj.NIMPORTE+obj.NPERCEPCION,2)) + ", ";
                    }
                    else
                    {
                        CadenaD += "" + IIfData(true, Math.Round(obj.NTOTRH+ obj.NPERCEPCION, 2), Math.Round(obj.NIMPORTE+obj.NPERCEPCION, 2)) + ", ";
                    }

                    CadenaD += "'" + Trim(obj.GASTOS_CUENTACON) + "', '" + funcBlanco(txtForPago) + "', '" + funcBlanco(txtRefSerie) + "', '" + funcBlanco(txtRefNro) + "',";
        Cadena = Cadena & "'" & funcBlanco(txtRefTpoDcto) & "', '" & txtTpoConv & "', " & FechS(VGFecTrb, Sqlf) & ", '" & txtTpoAnexo & "', '" & lblRuc & "', "
        Cadena = Cadena & "" & chkRegComp.Value & ", '" & funcBlanco(txtDEst) & "', " & txtDias & ", '" & Mid(chkIGVxAplic.Caption, 1, 1) & "', '" & txtCpto & "',"
        Cadena = Cadena & "" & FechS(dtpRefFecha, Sqlf) & ", " & txtTasaIGV & ", " & Round(Val(txtMontoIGV), 2) & ", " & Round(Val(nPorcen), 2) & ", '" & funcBlanco(txtRefAnexo) & "', "
        Cadena = Cadena & "" & IIf(fraHono.Visible, 1, 0) & ", " & Round(Val(txtIR4ta), 2) & ", " & Round(Val(txtIES), 2) & ", " & Round(Val(txttotal), 2) & ", " & Round(Val(txtBaseImp), 2) & ","


        Cadena = Cadena & "" & Round(Val(nValCIF), 2) & ", " & IIf(txtTpoConv = "FEC", FechS(dtpFecTC, Sqlf), FechS(dtpFecEmi, Sqlf)) & ", '" & msAnoMesProc & "', " & IIf(mbSalInic, 1, 0) & "," & ESNULO(txtImpPercep, 0) & ",'" & txNumDetraccion.Text & "'"
        If IsDate(MaskEdBox1) Then
           Cadena = Cadena & ",'" & Format(MaskEdBox1, "yyyy/dd/mm") & "'"
        Else
           Cadena = Cadena & ",''"
        End If
        Cadena = Cadena & ",'" & Trim(TxDocReferencia.Text) & "','" & IIf(Combo1.ListIndex = 1, "1", "0") & "',"


        If Val(txtMontoIGV) = 0 Then
          Cadena = Cadena & "0,0,0,'','',0,"
        Else
          Cadena = Cadena & cmbDetraccion.ListIndex & "," & Val(TxtTasaDet) & "," & Round(Val(txtTotalVta) * Val(TxtTasaDet) / 100, 2) & ",'" & txtTipServ.Text & "','" & txtTipOper.Text & "'," & Val(TxTImporteRef) & ","
        End If


        Cadena = Cadena & "'" & txt_RefCmp_Tipo.Text & "','" & txt_RefCmp_Serie.Text & "','" & txt_RefCmp_Documento & "',"
        If IsDate(txtFecDocRef) Then
          Cadena = Cadena & "'" & Format(txtFecDocRef, "yyyy/dd/mm") & "',"
        End If
        Cadena = Cadena & chk_RNoDomiciliado.Value & ",'" & txtOCompra.Text & "')"



                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    objConexion.getCon().Close();
                    objConexion.cerrarConexion();
                }
            }

        




  

    


    If Not(mrst_RegDctos.RecordCount< 1) Then reg = dgrData.Bookmark


    If mbEstado Then 'Nuevo registro
        cCorre = funcAutoNum
        If Trim(cCorre) = "NULL" Then
            Exit Sub
        End If
        'TxtOrden = cCorre


        Set rs = New ADODB.Recordset

        Cadena = "INSERT INTO COMPROBANTECAB ("
        Cadena = Cadena & "EMP_CODIGO,CORDEN,ANEX_CODIGO,ANEX_DESCRIPCION,TIPODOCU_CODIGO,CSERIE, "
        Cadena = Cadena & "CNUMERO, DEMISION, DVENCE, DRECEPCIO, TIPOMON_CODIGO, "
        Cadena = Cadena & "NIMPORTE, TIPOCAMBIO_VALOR, CDESCRIPC, RESPONSABLE_CODIGO, CESTADO, "
        Cadena = Cadena & "NSALDO, CCODCONTA, CFORMPAGO, CSERREFER, CNUMREFER, "
        Cadena = Cadena & "CTDREFER, CONVERSION_CODIGO, DREGISTRO, CTIPPROV, CNRORUC, "
        Cadena = Cadena & "ESTCOMPRA, CDESTCOMP, DIASPAGO, CIGVAPLIC, CCONCEPT, "
        Cadena = Cadena & "DFECREF, NTASAIGV, NIGV, NPORCE, CCODRUC, "
        Cadena = Cadena & "LHONOR, NIR4, NIES, NTOTRH, NBASEIMP, "
        Cadena = Cadena & "NVALCIF, DCONTAB, CAMESPROC, CSALDINI,NPERCEPCION,NUMRETRAC,"
        Cadena = Cadena & "FECRETRAC,CNUMORDCO,"
        Cadena = Cadena & "CO_L_RETE,LDETRACCION,NTASADETRACCION, DETRACCION,COD_SERVDETRACC,COD_TIPOOPERACION,"
        Cadena = Cadena & "nImporteRef, RCO_TIPO, RCO_SERIE, RCO_NUMERO,"
        If IsDate(txtFecDocRef) Then
           Cadena = Cadena & "RCO_FECHA,"
        End If
        Cadena = Cadena & "flg_RNTNODOMICILIADO,CAOCOMPRA) VALUES ("
        Cadena = Cadena & "'" & VGEMP_CODIGO & "','" & cCorre & "', '" & txtanexo & "', '" & lblAnexo & "', '" & txtTpoDcto & "', '" & txtserie & "',"
        Cadena = Cadena & "'" & txtnumero & "', " & FechS(dtpFecEmi, Sqlf) & ", " & FechS(dtpFecVenc, Sqlf) & ", " & FechS(dtpFecRecep, Sqlf) & ", '" & txtmoneda & "',"
        If Trim(txtDEst) = "007" Or Trim(txtDEst) = "008" Then
            Cadena = Cadena & "" & IIf(fraHono.Visible, Round(Val(txttotal), 2) - Round(Val(txtIR4ta), 2), Round(Val(txtTotalVta), 2)) & ", "
        ElseIf Trim(txtDEst) = "006" Then
            Cadena = Cadena & "" & IIf(fraHono.Visible, Round(Val(txttotal), 2) - (Round(Val(txtIR4ta), 2) + Round(Val(txtIES), 2)), Round(Val(txtTotalVta), 2)) & ", "
        Else
            Cadena = Cadena & "" & IIf(fraHono.Visible, Round(Val(txttotal), 2), Round(Val(txtTotalVta), 2)) & ", "
        End If
        Cadena = Cadena & "" & IIf(txtTpoConv = "ESP", txtTipEsp, txtTC) & ", '" & funcBlanco(TxtGlosa) & "', '" & txtResp & "', '" & sEstado & "', "
        If Trim(txtDEst) = "007" Or Trim(txtDEst) = "008" Then
            Cadena = Cadena & "" & IIf(fraHono.Visible, Round(Val(txttotal) + Val(txtImpPercep), 2) - Round(Val(txtIR4ta), 2), Round(Val(txtTotalVta) + Val(txtImpPercep), 2)) & ", "
        ElseIf Trim(txtDEst) = "006" Then
            Cadena = Cadena & "" & IIf(fraHono.Visible, Round(Val(txttotal) + Val(txtImpPercep), 2) - (Round(Val(txtIR4ta), 2) + Round(Val(txtIES), 2)), Round(Val(txtTotalVta) + Val(txtImpPercep), 2)) & ", "
        Else
            Cadena = Cadena & "" & IIf(fraHono.Visible, Round(Val(txttotal) + Val(txtImpPercep), 2), Round(Val(txtTotalVta) + Val(txtImpPercep), 2)) & ", "
        End If
        Cadena = Cadena & "'" & Trim(msCtaGasto) & "', '" & funcBlanco(txtForPago) & "', '" & funcBlanco(txtRefSerie) & "', '" & funcBlanco(txtRefNro) & "',"
        Cadena = Cadena & "'" & funcBlanco(txtRefTpoDcto) & "', '" & txtTpoConv & "', " & FechS(VGFecTrb, Sqlf) & ", '" & txtTpoAnexo & "', '" & lblRuc & "', "
        Cadena = Cadena & "" & chkRegComp.Value & ", '" & funcBlanco(txtDEst) & "', " & txtDias & ", '" & Mid(chkIGVxAplic.Caption, 1, 1) & "', '" & txtCpto & "',"
        Cadena = Cadena & "" & FechS(dtpRefFecha, Sqlf) & ", " & txtTasaIGV & ", " & Round(Val(txtMontoIGV), 2) & ", " & Round(Val(nPorcen), 2) & ", '" & funcBlanco(txtRefAnexo) & "', "
        Cadena = Cadena & "" & IIf(fraHono.Visible, 1, 0) & ", " & Round(Val(txtIR4ta), 2) & ", " & Round(Val(txtIES), 2) & ", " & Round(Val(txttotal), 2) & ", " & Round(Val(txtBaseImp), 2) & ","


        Cadena = Cadena & "" & Round(Val(nValCIF), 2) & ", " & IIf(txtTpoConv = "FEC", FechS(dtpFecTC, Sqlf), FechS(dtpFecEmi, Sqlf)) & ", '" & msAnoMesProc & "', " & IIf(mbSalInic, 1, 0) & "," & ESNULO(txtImpPercep, 0) & ",'" & txNumDetraccion.Text & "'"
        If IsDate(MaskEdBox1) Then
           Cadena = Cadena & ",'" & Format(MaskEdBox1, "yyyy/dd/mm") & "'"
        Else
           Cadena = Cadena & ",''"
        End If
        Cadena = Cadena & ",'" & Trim(TxDocReferencia.Text) & "','" & IIf(Combo1.ListIndex = 1, "1", "0") & "',"


        If Val(txtMontoIGV) = 0 Then
          Cadena = Cadena & "0,0,0,'','',0,"
        Else
          Cadena = Cadena & cmbDetraccion.ListIndex & "," & Val(TxtTasaDet) & "," & Round(Val(txtTotalVta) * Val(TxtTasaDet) / 100, 2) & ",'" & txtTipServ.Text & "','" & txtTipOper.Text & "'," & Val(TxTImporteRef) & ","
        End If


        Cadena = Cadena & "'" & txt_RefCmp_Tipo.Text & "','" & txt_RefCmp_Serie.Text & "','" & txt_RefCmp_Documento & "',"
        If IsDate(txtFecDocRef) Then
          Cadena = Cadena & "'" & Format(txtFecDocRef, "yyyy/dd/mm") & "',"
        End If
        Cadena = Cadena & chk_RNoDomiciliado.Value & ",'" & txtOCompra.Text & "')"



        rs.Open Cadena, cConexCom, adOpenKeyset, adLockOptimistic



        If Check1.Value = 1 Then
            If Not ExisteElem(1, CN_CTAPAG, "comprobantecab", "NPERCEPCION") Then
               CN_CTAPAG.Execute "ALTER TABLE comprobantecab ADD NPERCEPCION NUMERIC(15,6) NULL"
            End If


            If Not ExisteElem(1, CN_CTAPAG, "comprobantecab", "LDETRACCION") Then
               CN_CTAPAG.Execute "ALTER TABLE comprobantecab ADD LDETRACCION BIT DEFAULT 0"
            End If


            If Not ExisteElem(1, CN_CTAPAG, "comprobantecab", "NTASADETRACCION") Then
               CN_CTAPAG.Execute "ALTER TABLE comprobantecab ADD NTASADETRACCION NUMERIC(15,6) DEFAULT 0"
            End If


            If Not ExisteElem(1, CN_CTAPAG, "comprobantecab", "NIMPORTEREF") Then
               CN_CTAPAG.Execute "ALTER TABLE comprobantecab ADD NIMPORTEREF NUMERIC(15,6) DEFAULT 0"
            End If


            If IsDate(MaskEdBox1) Then
                Call Trasferencia_CxP(cCorre, txtTpoDcto, txtserie, txtnumero, Trim(txNumDetraccion), IIf(cmbDetraccion.ListIndex = 1, True, False), Val(TxtTasaDet), Val(TxTImporteRef), CDate(MaskEdBox1))
            Else
                Call Trasferencia_CxP(cCorre, txtTpoDcto, txtserie, txtnumero, Trim(txNumDetraccion), IIf(cmbDetraccion.ListIndex = 1, True, False), Val(TxtTasaDet), Val(TxTImporteRef))
            End If
        End If


    Else 'Solo actualizar
        'Me quede en que se debe deja r chequear el dcto pero no dejarlo modificar o grbar el dcto contabilizado
        cCorre = TxtOrden
        If mrst_TRegDctos!cEstado <> "0" And mrst_TRegDctos!cEstado <> "1" Then
            Screen.MousePointer = 1
            MsgBox "El estado del documento no permite su modificación.", vbExclamation, "Mensaje"
            Cadena = "UPDATE COMPROBANTECAB SET CAOCOMPRA='" & txtOCompra.Text & "',CNUMORDCO='" & Trim(TxDocReferencia.Text) & "', "
            Cadena = Cadena & " NUMRETRAC='" & txNumDetraccion & "'"
            If IsDate(MaskEdBox1) Then
                Cadena = Cadena & ",FECRETRAC=" & FechS(MaskEdBox1, Sqlf) & ""
            Else
                Cadena = Cadena & ",FECRETRAC=NUll"
            End If
            Cadena = Cadena & ",LDETRACCION=" & cmbDetraccion.ListIndex & ",NTASADETRACCION=" & Val(TxtTasaDet) & ",DETRACCION=" & Round(Val(txtTotalVta) * Val(TxtTasaDet) / 100, 2) & ",COD_SERVDETRACC='" & txtTipServ.Text & "',COD_TIPOOPERACION='" & txtTipOper.Text & "' WHERE CAMESPROC='" & msAnoMesProc & "' AND CORDEN='" & cCorre & "'"
            cConexCom.Execute Cadena
            Exit Sub
        End If
        'Antes de modificar verificar ante si el saldo actual del dcto es igual al importe
        'SI es asi entonces si se puede modificar
        'Sino entonces debe eliminar lo programado y lo cancelado para que se restaure el saldo
        Set rst_Estado = New ADODB.Recordset
        rst_Estado.Open "SELECT NIMPORTE, NSALDO, NMONTPROG FROM COMPROBANTECAB WHERE CAMESPROC='" & msAnoMesProc & "' AND CORDEN='" & _
                    cCorre & "'", cConexCom, adOpenForwardOnly, adLockReadOnly
        If Not rst_Estado.EOF Then
            'If rst_Estado!NIMPORTE <> rst_Estado!NSALDO Then
            If rst_Estado!NMONTPROG <> 0 Then
                Screen.MousePointer = 1
                'MsgBox "No es posible modificar este documento por tener un saldo diferente al importe.", vbExclamation, "Mensaje"
                MsgBox "Este documento no puede ser modificado ya que se encuentra programado y/o cancelado." & Chr(13) & _
                            "El monto de programación es diferente de 0.", vbInformation, "Mensaje"

                Set rst_Estado = Nothing
                Exit Sub
            End If
        Else
            MsgBox "Documento no ha sido encontrado.", vbExclamation, "Mensaje"
            Set rst_Estado = Nothing
            Screen.MousePointer = 1
            Exit Sub
        End If


'        If txtDest = "002" Then
'            'Guarda el porcentaje del Dest grabado del dcto sólo cuando Destino = "002"
'            fraOperGrav.Visible = True
'            cmdOperGravAceptar.SetFocus
'            nPorcen = txtOperGrav
'        End If


        If txtTpoConv<> "ESP" Then
           If funcValorTipoCambio(IIf(txtTpoConv = "FEC", dtpFecTC.Value, dtpFecEmi.Value), IIf(txtTpoConv = "FEC", "VTA", txtTpoConv), TipCam, TipCamEq) Then
               txtTC = TipCam
            Else
                Screen.MousePointer = 1
                txtTC = "0.000"
                MsgBox "No se encontró el valor de tipo de cambio.", vbExclamation, "Mensaje"
                Exit Sub
            End If
        End If


        Call EliminarTrasfCxP(txtTpoAnexo, txtanexo, txtTpoDcto, txtserie, txtnumero, msAnoMesProc)


        Dim CADSQL As String
        CADSQL = "UPDATE COMPROBANTECAB SET ANEX_CODIGO='" & txtanexo & "', ANEX_DESCRIPCION='" & lblAnexo & "'"
        CADSQL = CADSQL & ", TIPODOCU_CODIGO='" & txtTpoDcto & "', CSERIE='" & txtserie & "'"
        CADSQL = CADSQL & ", CNUMERO='" & txtnumero & "', DEMISION=" & FechS(dtpFecEmi, Sqlf) & ""
        CADSQL = CADSQL & ", DVENCE=" & FechS(dtpFecVenc, Sqlf) & ", DRECEPCIO=" & FechS(dtpFecRecep, Sqlf) & ""
        CADSQL = CADSQL & ", TIPOMON_CODIGO='" & txtmoneda & "', NIMPORTE=" & IIf(fraHono.Visible, Round(Val(txttotal), 2) - Round(Val(txtIR4ta), 2), Round(Val(txtTotalVta), 2))
        CADSQL = CADSQL & ", TIPOCAMBIO_VALOR=" & IIf(txtTpoConv = "ESP", Round(Val(txtTipEsp), 3), Round(Val(txtTC), 3)) & ", CDESCRIPC='" & funcBlanco(TxtGlosa) & "'"
        CADSQL = CADSQL & ", RESPONSABLE_CODIGO='" & txtResp & "', CESTADO='" & sEstado & "', CCODCONTA='" & funcBlanco(Trim(msCtaGasto)) & "'"
        CADSQL = CADSQL & ", CFORMPAGO='" & txtForPago & "', CSERREFER='" & funcBlanco(txtRefSerie) & "', CNUMREFER='" & funcBlanco(txtRefNro) & "'"
        CADSQL = CADSQL & ", CTDREFER='" & funcBlanco(txtRefTpoDcto) & "', CONVERSION_CODIGO='" & txtTpoConv & "', DREGISTRO=" & FechS(CDate(VGFecTrb), Sqlf) & ""
        CADSQL = CADSQL & ", CTIPPROV='" & txtTpoAnexo & "', CNRORUC='" & lblRuc & "', ESTCOMPRA=" & chkRegComp.Value
        CADSQL = CADSQL & ", CDESTCOMP='" & funcBlanco(txtDEst) & "', DIASPAGO=" & txtDias & ", NIGV=" & Round(Val(txtMontoIGV), 2)
        CADSQL = CADSQL & ", CIGVAPLIC='" & Mid(chkIGVxAplic.Caption, 1, 1) & "', CCONCEPT='" & txtCpto & "', DFECREF=" & FechS(dtpRefFecha, Sqlf) & ""
        CADSQL = CADSQL & ", NTASAIGV=" & txtTasaIGV & ", NPORCE=" & Round(Val(nPorcen), 2) & ", CCODRUC='" & funcBlanco(txtRefAnexo) & "'"
        CADSQL = CADSQL & ", LHONOR=" & IIf(fraHono.Visible, 1, 0) & ", NIR4=" & Round(Val(txtIR4ta), 2) & ", NIES=" & Round(Val(txtIES), 2)
        CADSQL = CADSQL & ", NTOTRH=" & Round(Val(txttotal), 2) & ", NSALDO=" & IIf(fraHono.Visible, Round(Val(txttotal) - Round(Val(txtIR4ta), 2), 2), Round(Val(txtTotalVta), 2)) & ", NBASEIMP=" & Round(Val(txtBaseImp), 2)
        '2003/10/16: ' cambia
        'CADSQL = CADSQL & ", NVALCIF=" & Round(Val(nValCIF), 2) & ", DCONTAB=" & IIf(txtTpoConv = "FEC", FechS(CDate(dtpFecTC.Value), Sqlf), FechS(CDate(VGFecTrb), Sqlf)) & ",LPASOIMP=" & Check1.Value & " "
        CADSQL = CADSQL & ", NVALCIF=" & Round(Val(nValCIF), 2) & ", DCONTAB=" & IIf(txtTpoConv = "FEC", FechS(CDate(dtpFecTC.Value), Sqlf), FechS(CDate(VGFecTrb), Sqlf)) & ",NPERCEPCION=" & ESNULO(txtImpPercep, 0) & ",NUMRETRAC='" & txNumDetraccion & "' "
        If IsDate(MaskEdBox1) Then
            CADSQL = CADSQL & ",FECRETRAC=" & FechS(MaskEdBox1, Sqlf) & ""
        Else
            CADSQL = CADSQL & ",FECRETRAC=NUll"
        End If
        CADSQL = CADSQL & " ,CNUMORDCO='" & Trim(TxDocReferencia) & "',CO_L_RETE='" & IIf(Combo1.ListIndex = 1, "1", "0") & "',LDETRACCION=" & cmbDetraccion.ListIndex & ",NTASADETRACCION=" & Val(TxtTasaDet) & ","
        CADSQL = CADSQL & " NIMPORTEREF=" & IIf(cmbDetraccion.ListIndex = 0, 0, Val(TxTImporteRef)) & ", "


        CADSQL = CADSQL & "RCO_TIPO='" & txt_RefCmp_Tipo.Text & "',RCO_SERIE='" & txt_RefCmp_Serie.Text & "',RCO_NUMERO='" & txt_RefCmp_Documento & "',RCO_FECHA='" & Format(txtFecDocRef, "yyyy/dd/mm") & "',"
        CADSQL = CADSQL & "flg_RNTNODOMICILIADO=" & chk_RNoDomiciliado.Value & ",CAOCOMPRA='" & txtOCompra.Text & "'"
        CADSQL = CADSQL & " WHERE CAMESPROC='" & msAnoMesProc & "' AND CORDEN='" & cCorre & "' "


        cConexCom.Execute CADSQL


        If Check1.Value = 1 Then
                If Not ExisteElem(1, CN_CTAPAG, "comprobantecab", "NPERCEPCION") Then
                   CN_CTAPAG.Execute "ALTER TABLE comprobantecab ADD NPERCEPCION NUMERIC(15,6) NULL"
                End If
                If IsDate(MaskEdBox1) Then
                    Call Trasferencia_CxP(cCorre, txtTpoDcto, txtserie, txtnumero, Trim(txNumDetraccion), IIf(cmbDetraccion.ListIndex = 1, True, False), Val(TxtTasaDet), Val(TxTImporteRef), CDate(MaskEdBox1))
                Else
                    Call Trasferencia_CxP(cCorre, txtTpoDcto, txtserie, txtnumero, Trim(txNumDetraccion), IIf(cmbDetraccion.ListIndex = 1, True, False), Val(TxtTasaDet), Val(TxTImporteRef))
                End If
        End If
    End If
        }
        */

        public void create(Comprobante obj)
        {

        }
        public decimal IIfData(bool verificacion, decimal data1, decimal data2)
        {
            if (verificacion)
            {
                return data1;
            }
            else
            {
                return data2;
            }
        }

        public string verificaDoc_cxc(string anex, string doc, string Serie, string Num)
        {

            
            string sql = "select ANEX_cODIGO,TIPODOCU_CODIGO,CSERIE,CNUMERO,NIMPORTE,NSALDO,LCANJEADO,NMONTPROG from [014BDCTAPAG].[dbo].[ComprobanteCab]";
            sql += " where ANEX_cODIGO='" + anex + "' and TIPODOCU_CODIGO='" + doc + "' and CSERIE='" + Serie + "' and CNUMERO='" + Num + "' ";

            try
            {
                comando = new SqlCommand(sql, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                if (read.Read())
                {
                    sql = "SELECT * FROM [014BDCTAPAG].[dbo].[ProgramacionDet] WHERE CCODPROVE='" + anex + "' ";
                    sql += " AND CCODDOCUM='" + doc + "' AND CSERIE='" + Serie + "' AND CNUMERO='" + Num + "' ";
                    comando = new SqlCommand(sql, objConexion.getCon());
                    objConexion.getCon().Open();
                    SqlDataReader readinterno = comando.ExecuteReader();
                    if (readinterno.Read())
                    {
                        return "El Documento se Encuentra Programado en Cuentas x Pagar";
                    }                   
                    decimal saldo = Conversion.ParseDecimal(read[5].ToString());
                    decimal importe = Conversion.ParseDecimal(read[4].ToString());
                    bool canjeado= Conversion.ParseBool(read[6].ToString());
                    if (saldo == 0 || saldo < importe)
                    {
                        if (canjeado)
                        {
                            return "El Documento Esta Canjeado en Cuentas x Pagar";
                        }
                        else
                        {
                            return "El Documento Esta Canjeado en Cuentas x Pagar";

                        }

                      
                    }
                    
                    return "El Documento Esta en Cuentas x Pagar";
                }
                else
                {
                    return "";
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
                  
        }


        public string funcAutoNum( string msAnoMesProc)
        {
            string cadena = "SELECT Concepto_Logico FROM CONCEPTOGRAL WHERE Concepto_Codigo='NUMEAUTO'";
            try
            {
                comando = new SqlCommand(cadena, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                bool hayRegistros = read.Read();
                if (hayRegistros)
                {
                    bool codigo = Conversion.ParseBool( read[0].ToString());
                     cadena = "SELECT MAX(CORDEN) AS MAXORDEN FROM COMPROBANTECAB WHERE CAMESPROC = '"+ msAnoMesProc+"'";
                    comando = new SqlCommand(cadena, objConexion.getCon());
                    objConexion.getCon().Open();
                    SqlDataReader readd = comando.ExecuteReader();
                    string last = readd[0].ToString();
                    int nextDocumet = 0;
                    if (last != "")
                    {
                        nextDocumet = int.Parse(last) + 1;
                    }
                  
                    string next = nextDocumet.ToString("00000.##");
                    if (codigo)
                    {
                        return next;
                    }
                    else
                    {
                        return "";
                    }
                    //return next;
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }


            return "";
        }




        public void delete(Comprobante obj)
        {
            throw new NotImplementedException();
        }

        public bool find(Comprobante obj)
        {
            throw new NotImplementedException();
        }

        public List<Comprobante> findAll()
        {

            List<Comprobante> listComprobantes = new List<Comprobante>();
            DateTime date = DateTime.Now; 
      
            string anios = date.Year.ToString("0000.##"); 
            string mes = date.Month.ToString("00.##");
            string msAnoMesProc = anios + mes;
            //string findAll = $"SELECT * FROM {table} WHERE CAMESPROC='" + msAnoMesProc +"' AND CSALDINI=0";

            string conexion = Conexion.CadenaGeneral("014", "BDCOMUN", "COMPROBANTECAB");

            string findAll = $"SELECT * FROM  {conexion}  WHERE CAMESPROC='" + msAnoMesProc + "' AND CSALDINI=0";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    Comprobante comp = new Comprobante();
                    comp.EMP_CODIGO = read[0].ToString();
                    comp.CORDEN = read[1].ToString();
                    comp.ANEX_CODIGO = read[2].ToString();
                    comp.ANEX_DESCRIPCION = read[3].ToString();
                    comp.TIPODOCU_CODIGO = read[4].ToString();
                    comp.CSERIE = read[5].ToString();
                    comp.CNUMERO = read[6].ToString();
                    comp.DEMISION = Conversion.ParseDateTime(read[7].ToString());
                    comp.DVENCE = Conversion.ParseDateTime(read[8].ToString());
                    comp.DRECEPCIO = Conversion.ParseDateTime(read[9].ToString());
                    comp.TIPOMON_CODIGO = read[10].ToString();
                    comp.NIMPORTE = Conversion.ParseDecimal(read[11].ToString());
                    comp.TIPOCAMBIO_VALOR = Conversion.ParseDecimal(read[12].ToString());
                    comp.CNUMORDCO = read[13].ToString();
                    comp.CDESCRIPC = read[14].ToString();
                    comp.RESPONSABLE_CODIGO = read[15].ToString();
                    comp.CESTADO = read[16].ToString();
                    comp.NSALDO = Conversion.ParseDecimal(read[17].ToString());
                    comp.NMONTPROG = Conversion.ParseDecimal(read[18].ToString());
                    comp.LCANJEADO = Conversion.ParseBool(read[19].ToString());
                    comp.CCODCONTA = read[20].ToString();
                    comp.CFORMPAGO = read[21].ToString();
                    comp.CSERREFER = read[22].ToString();
                    comp.CNUMREFER = read[23].ToString();
                    comp.CTDREFER = read[24].ToString();
                    comp.SUBDIARIO_CODIGO = read[25].ToString();
                    comp.CONVERSION_CODIGO = read[26].ToString();
                    comp.CTIPPROV = read[27].ToString();
                    comp.CCTADEST = read[28].ToString();
                    comp.CCOSTO = read[29].ToString();
                    comp.CNRORUC = read[30].ToString();
                    comp.CONTAB = Conversion.ParseBool(read[31].ToString());
                    comp.CTIPO = read[32].ToString();
                    comp.ESTCOMPRA = Conversion.ParseBool(read[33].ToString());
                    comp.CDESTCOMP = read[34].ToString();
                    comp.COMPCON = read[35].ToString();
                    comp.CSALDINI = Conversion.ParseBool(read[36].ToString());
                    comp.CODCAJCH = read[37].ToString();
                    comp.DIASPAGO = Conversion.ParseDecimal(read[38].ToString());
                    comp.CIGVAPLIC = read[39].ToString();
                    comp.CAMESPROC = read[40].ToString();
                    comp.CCONCEPT = read[41].ToString();
                    comp.DFECREF = Conversion.ParseDateTime(read[42].ToString());
                    comp.NIGV = Conversion.ParseDecimal(read[43].ToString());
                    comp.NTASAIGV = Conversion.ParseDecimal(read[44].ToString());
                    comp.NCAMBESP = Conversion.ParseDecimal(read[45].ToString());
                    comp.DFECCA = Conversion.ParseDateTime(read[46].ToString());
                    comp.LANULA = Conversion.ParseBool(read[47].ToString());
                    comp.NPORCE = Conversion.ParseDecimal(read[48].ToString());
                    comp.CCODRUC = read[49].ToString();
                    comp.CRAZON = read[50].ToString();
                    comp.LREGCO = Conversion.ParseBool(read[51].ToString());
                    comp.LHONOR = Conversion.ParseBool(read[52].ToString());
                    comp.NIR4 = Conversion.ParseDecimal(read[53].ToString());
                    comp.NIES = Conversion.ParseDecimal(read[54].ToString());
                    comp.NTOTRH = Conversion.ParseDecimal(read[55].ToString());
                    comp.NRENTA2DA = Conversion.ParseDecimal(read[56].ToString());
                    comp.NISC = Conversion.ParseDecimal(read[57].ToString());
                    comp.BANCO_CODIGO = read[58].ToString();
                    comp.CAGENCIA = read[59].ToString();
                    comp.Cnltbco = read[60].ToString();
                    comp.NVALCIF = Conversion.ParseDecimal(read[61].ToString());
                    comp.NBASEIMP = Conversion.ParseDecimal(read[62].ToString());
                    comp.NINAFECTO = Conversion.ParseDecimal(read[63].ToString());
                    comp.DCONTAB = Conversion.ParseDateTime(read[64].ToString());
                    comp.NTASAPERCEPCION = Conversion.ParseDecimal(read[65].ToString());
                    comp.NPERCEPCION = Conversion.ParseDecimal(read[66].ToString());
                    comp.NTASARETENCION = Conversion.ParseDecimal(read[67].ToString());
                    comp.NRETENCION = Conversion.ParseDecimal(read[68].ToString());
                    comp.CO_L_RETE = read[69].ToString();
                    comp.CODDETRACC = read[70].ToString();
                    comp.NUMRETRAC = read[71].ToString();
                    comp.FECRETRAC = Conversion.ParseDateTime(read[72].ToString());
                    comp.LPASOIMP = Conversion.ParseBool(read[73].ToString());
                    comp.LDETRACCION = Conversion.ParseBool(read[74].ToString());
                    comp.NTASADETRACCION = Conversion.ParseDecimal(read[75].ToString());
                    comp.DETRACCION = Conversion.ParseDecimal(read[76].ToString());
                    comp.COD_SERVDETRACC = read[77].ToString();
                    comp.COD_TIPOOPERACION = read[78].ToString();
                    comp.NIMPORTEREF = Conversion.ParseDecimal(read[79].ToString());
                    comp.RCO_TIPO = read[80].ToString();
                    comp.RCO_SERIE = read[81].ToString();
                    comp.RCO_NUMERO = read[82].ToString();
                    comp.RCO_FECHA = Conversion.ParseDateTime(read[83].ToString());
                    comp.DREGISTRO = Conversion.ParseDateTime(read[84].ToString());
                    comp.USERREG = read[85].ToString();
                    comp.flg_RNTNODOMICILIADO = Conversion.Parseint(read[86].ToString());
                    comp.CAOCOMPRA = read[87].ToString();

                    comp.codigo = comp.CORDEN + comp.CAMESPROC;
                    comp.documento = comp.TIPODOCU_CODIGO + ' ' + comp.CSERIE + " - " + comp.CNUMERO;

                    listComprobantes.Add(comp);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return listComprobantes;
        }

        public List<Gasto> findAllGastos()
        {
            List<Gasto> listGastos = new List<Gasto>();

            string conexion = Conexion.CadenaGeneral("014", "BDCTAPAG", "Gastos");
            string findAll = $"SELECT Gastos_Codigo, Gastos_Descripcion from {conexion} ";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    Gasto gasto = new Gasto();
                    gasto.Gastos_Codigo = read[0].ToString();
                    gasto.Gastos_Descripcion = read[1].ToString();
                    listGastos.Add(gasto);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return listGastos;
        }
        public Gasto findAllGastosDetail(string codigo)
        {
           

            string conexion = Conexion.CadenaGeneral("014", "BDCTAPAG", "Gastos");
            string findAll = $"GASTOS_CODIGO, GASTOS_DESCRIPCION, GASTOS_MONEDA, GASTOS_HONORARIO, GASTOS_CUENTACON,GASTOS_DSCTO1, GASTOS_DSCTO2 FROM {conexion} WHERE GASTOS_CODIGO = '" + codigo + "'" ;
            Gasto gasto = new Gasto();
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                if (read.Read())
                {                    
                    gasto.Gastos_Codigo = read[0].ToString();
                    gasto.Gastos_Descripcion = read[1].ToString();
                    gasto.Gastos_Moneda= read[2].ToString();
                    gasto.Gastos_Honorario= read[3].ToString();
                    gasto.Gastos_CuentaCon= read[4].ToString();
                    gasto.Gastos_Dscto1= read[5].ToString();
                    gasto.Gastos_Dscto2= read[6].ToString();
                   
                }
                return gasto;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            
        }
       

        public void update(Comprobante obj)
        {
            throw new NotImplementedException();
        }
    }
}