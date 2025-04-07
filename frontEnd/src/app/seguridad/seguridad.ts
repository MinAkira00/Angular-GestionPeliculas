export interface credencialesUsuario{
    email: string;
    password: string;
}

export interface respuestaAutenticacion {
    token: string;
    expiracion: Date;
}

export interface usuarioDto {
    id: string;
    email: string;
}