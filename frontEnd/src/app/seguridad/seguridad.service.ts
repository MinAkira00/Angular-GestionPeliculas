import { Injectable } from '@angular/core';
import { credencialesUsuario, respuestaAutenticacion } from './seguridad';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Expansion } from '@angular/compiler';

@Injectable({
  providedIn: 'root'
})
export class SeguridadService {

  constructor(private httpClient: HttpClient) { }
  apiUrl = environment.apiURL + 'cuentas'
  private readonly llaveToken = 'token'
  private readonly llaveExpiracion = 'token-expiracion'

  estaLogueado(): boolean {
    const token = localStorage.getItem(this.llaveToken)
    if(!token){
      return false;
    }
    const expiracion = localStorage.getItem(this.llaveExpiracion)
    const expiracionFecha = new Date(expiracion)

    if(expiracionFecha <= new Date()){
      this.logout()
      return false;
    }

    return true;
  }

  logout(){
    localStorage.removeItem(this.llaveToken);
    localStorage.removeItem(this.llaveExpiracion)
  }

  
  obtenerRol():string {
    return ''
  }

  obtenerCampoJWT(campo: string): string {
    const token = localStorage.getItem(this.llaveToken)
    if(!token){return ''}
    var dataToken = JSON.parse(atob(token.split('.')[1]))
    return dataToken[campo]
  }

  registrar(credenciales: credencialesUsuario): Observable<respuestaAutenticacion> {
    return this.httpClient.post<respuestaAutenticacion>(this.apiUrl + '/crear', credenciales)
  }

  login(credenciales: credencialesUsuario): Observable<respuestaAutenticacion> {
    return this.httpClient.post<respuestaAutenticacion>(this.apiUrl + '/login', credenciales)
  }

  guardarToken(respuestaAutenticacion: respuestaAutenticacion){
    localStorage.setItem(this.llaveToken, respuestaAutenticacion.token)
    localStorage.setItem(this.llaveExpiracion, respuestaAutenticacion.expiracion.toString())
  }
}
