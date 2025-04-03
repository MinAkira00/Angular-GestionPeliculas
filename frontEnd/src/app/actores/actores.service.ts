import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { actorCreacionDTO, actorDTO } from './actor';
import { HttpClient, HttpParams } from '@angular/common/http';
import { formatearFecha } from '../utilidades/utilidades';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ActoresService {

  constructor(private http: HttpClient) { }

  private apiURL = environment.apiURL + 'actores';

  public obtenerTodos(pagina: number, cantidadElementosAMostrar: number): Observable<any> {
      let params = new HttpParams();
      params = params.append('pagina', pagina.toString());
      params = params.append('RecordsPorPagina', cantidadElementosAMostrar.toString());
      return this.http.get<actorDTO[]>(this.apiURL, {observe:'response', params})
  }
  
  public obtenerPorId(id: number): Observable<actorDTO>{
      return this.http.get<actorDTO>(`${this.apiURL}/${id}`);
  }

  public crear(actor: actorCreacionDTO){
    const formData = this.construirFormData(actor)
    return this.http.post(this.apiURL, formData);
  }

  public editar(id:number, actor: actorCreacionDTO){
    const formData = this.construirFormData(actor)
   formData.forEach((value,key) => {
    console.log(`${key}:`,value)
   })
    return this.http.put(`${this.apiURL}/${id}`, formData)
  }

  private construirFormData(actor: actorCreacionDTO): FormData {
    const formData = new FormData();
    formData.append('nombre', actor.nombre);
    if(actor.fechaNacimiento){
      formData.append('fechaNacimiento', formatearFecha(actor.fechaNacimiento));
    }
    if(actor.foto){
      formData.append('foto', actor.foto)
    } else if (!actor.foto){
      formData.append('foto', '')
    }

    if(actor.biografia){
      formData.append('biografia', actor.biografia);
    }

    return formData;
  }

 

  public borrar(id: number){
    return this.http.delete(`${this.apiURL}/${id}`);
  }
}
