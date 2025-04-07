import { HttpResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { usuarioDto } from '../seguridad';
import { SeguridadService } from '../seguridad.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-indice-usuarios',
  templateUrl: './indice-usuarios.component.html',
  styleUrls: ['./indice-usuarios.component.css']
})
export class IndiceUsuariosComponent implements OnInit {
constructor(private seguridadService: SeguridadService) { }

  usuarios: usuarioDto[];
  columnasAMostrar = ['nombre','acciones'];
  cantidadTotalRegistros;
  paginaActual = 1;
  cantidadRegistrosAMostrar = 10;

  ngOnInit(): void {
    this.cargarRegistros(this.paginaActual, this.cantidadRegistrosAMostrar);
  }
  
  cargarRegistros(pagina: number, cantidadElementosAMostrar){
    this.seguridadService.obtenerUsuario(pagina, cantidadElementosAMostrar)
      .subscribe((respuesta: HttpResponse<usuarioDto[]>) => {
        this.usuarios = respuesta.body;
        this.cantidadTotalRegistros = respuesta.headers.get("cantidadTotalRegistros");

    }, error => console.error(error));
  }

  actualizarPaginacion(datos: PageEvent){
    this.paginaActual = datos.pageIndex + 1;
    this.cantidadRegistrosAMostrar = datos.pageSize;
    this.cargarRegistros(this.paginaActual, this.cantidadRegistrosAMostrar);
  }

  hacerAdmin(usuarioId: string){
    this.seguridadService.hacerAdmin(usuarioId)
      .subscribe(()=> Swal.fire('Exitoso', "La operacion se ha realizado", 'success'))
  }

  removerAdmin(usuarioId: string){
    this.seguridadService.removerAdmin(usuarioId)
      .subscribe(()=> Swal.fire('Exitoso', "La operacion se ha realizado", 'success'))
  }
}
