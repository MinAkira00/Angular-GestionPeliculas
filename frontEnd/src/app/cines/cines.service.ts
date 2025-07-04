import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { cineCreacionDTO, cineDTO } from './cine';

@Injectable({
  providedIn: 'root'
})
export class CinesService {
 constructor(private http: HttpClient) { }
  private apiURL = environment.apiURL + 'cines';

  public obtenerTodos(pagina: number, cantidadElementosAMostrar: number): Observable<any> {
    let params = new HttpParams();
    params = params.append('pagina', pagina.toString());
    params = params.append('RecordsPorPagina', cantidadElementosAMostrar.toString());
    return this.http.get<cineDTO[]>(this.apiURL, {observe:'response', params})
  }

  public obtenerPorId(id: number): Observable<cineDTO>{
    return this.http.get<cineDTO>(`${this.apiURL}/${id}`);
  }

  public crear(cine: cineCreacionDTO){
    return this.http.post(this.apiURL, cine);
  }

  public editar(id:number, cine: cineCreacionDTO){
    return this.http.put(`${this.apiURL}/${id}`, cine)
  }

  public borrar(id: number){
    return this.http.delete(`${this.apiURL}/${id}`);
  }
}
