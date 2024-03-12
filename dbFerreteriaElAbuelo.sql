-- TRABAJO EXP FORMATIVA - SISTEMA DE VENTAS CON C# MVC FERRETERIA EL ABUELO
-- SE CREARA BASE DE DATOS / TABLAS / PROCEDIMIENTOS ALMACENADOS

--Creación de la database
Create database dbFerreteriaElAbuelo
GO

USE dbFerreteriaElAbuelo
GO


-- Implementar la logica en la creación de tablas 

--Tabla Categoria
CREATE TABLE CATEGORIA(
IdCategoria int primary key identity,
Descripcion varchar(100),
Activo bit default 1,
FechaRegistro datetime default getdate()
)
GO

--Tabla Marca
CREATE TABLE MARCA(
IdMarca int primary key identity,
Descripcion varchar(100),
Activo bit default 1,
FechaRegistro datetime default getdate()
)
GO


--Tabla Producto
CREATE TABLE PRODUCTO(
IdProducto int primary key identity,
Nombre varchar(500),
Descripcion varchar(500),
IdMarca int references MARCA(IdMarca),
IdCategoria int references CATEGORIA(IdCategoria),
Precio decimal(10,2) default 0,
Stock int,
RutaImagen varchar(100),
NombreImagen varchar(100),
Activo bit default 1,
FechaRegistro datetime default getdate()
)
GO


--Tabla Cliente
CREATE TABLE CLIENTE(
IdCliente int primary key identity,
Nombres varchar(100),
Apellidos varchar(100),
Correo varchar(100),
Clave varchar(150),
Reestablecer bit default 0,   -- sistema crea por default la contraseña y nosotros lo reestablecemos
FechaRegistro datetime default getdate()
)
GO

--Tabla Carrito
CREATE TABLE CARRITO(
IdCarrito int primary key identity,
IdCliente int references CLIENTE(IdCliente),
IdProducto int references PRODUCTO(IdProducto),
Cantidad int
)
GO

--Tabla Venta
CREATE TABLE VENTA(
IdVenta int primary key identity,
IdCliente int references CLIENTE(IdCliente),
TotalProducto int,
MontoTotal decimal(10,2),
Contacto varchar(50),
IdDistrito varchar(10),
Telefono varchar(50),
Direccion varchar(500),
IdTransaccion varchar(50),   --trabajar transaccion con paypal
FechaVenta datetime default getdate()
)
GO


--tabla Detalle Venta
CREATE TABLE DETALLE_VENTA(
IdDetalleVenta int primary key identity,
IdVenta int references VENTA(IdVenta),
IdProducto int references PRODUCTO(IdProducto),
Cantidad int,
Total decimal(10,2)
)
GO


--Table Usuario
CREATE TABLE USUARIO(
IdUsuario int primary key identity,
Nombres varchar(100),
Apellidos varchar(100),
Correo varchar(100),
Clave varchar(150),
Reestablecer bit default 1,
Activo bit default 1,
FechaRegistro datetime default getdate()
)
GO


--CREAMOS LAS TABLAS QUE CORRESPONDIENTE PARA LA UBICACIÓN DEL CLIENTE

--Tabla DEPARTAMENTO
CREATE TABLE DEPARTAMENTO(
IdDepartamento varchar(2) not null,
Descripcion varchar(45) not null
)
GO

--Tabla Provincia
CREATE TABLE PROVINCIA(
IdProvincia varchar(4) not null,
Descripcion varchar(45) not null,
IdDepartamento varchar(2) not null
)
GO


--Tabla Distrito
CREATE TABLE DISTRITO(
IdDistrito varchar(6) not null,
Descripcion varchar(45) not null,
IdProvincia varchar(4) not null,
IdDepartamento varchar(2) not null
)
GO



-- INSERTAMOS DATOS A LA TABLA

insert into USUARIO(Nombres,Apellidos,Correo,Clave) values ('omar','aguilar','test@example.com','ecd71870d1963316a97e3ac3408c9835ad8cf0f3c1bc703527c30265534f75ae')
go

select * from USUARIO
go

 insert into CATEGORIA(Descripcion) values 
 ('Industria'),
 ('Construcción'),
 ('Pinturas'),
 ('Proteccion - Equipo Vestuario'),
 ('Baño - Fontaneria'),
 ('Cerrajería'),
 ('Doméstico')
go


insert into MARCA(Descripcion) values
('KOPLAST'),
('CPP'),
('PAVCO'),
('TRUPER'),
('VIRUTEX'),
('TEKNO'),
('FORTE')
go

insert into DEPARTAMENTO(IdDepartamento,Descripcion)
values 
('01','Arequipa'),
('02','Ica'),
('03','Lima'),
('04','La Libertad')
go

insert into PROVINCIA(IdProvincia,Descripcion,IdDepartamento)
values
('0101','Arequipa','01'),
('0102','Camaná','01'),

--ICA - PROVINCIAS
('0201', 'Ica ', '02'),
('0202', 'Chincha ', '02'),

--LIMA - PROVINCIAS
('0301', 'Lima ', '03'),
('0302', 'Barranca ', '03'),

--La Libertad - PROVINCIAS
('0401', 'La Libertad ', '04'),
('0402', 'Trujillo ', '04')
go


insert into DISTRITO(IdDistrito,Descripcion,IdProvincia,IdDepartamento) values 
('010101','Nieva','0101','01'),
('010102', 'El Cenepa', '0101', '01'),
('010201', 'Camaná', '0102', '01'),
('010202', 'José María Quimper', '0102', '01'),

--ICA - DISTRITO
('020101', 'Ica', '0201', '02'),
('020102', 'La Tinguiña', '0201', '02'),
('020201', 'Chincha Alta', '0202', '02'),
('020202', 'Alto Laran', '0202', '02'),

--LIMA - DISTRITO
('030101', 'Lima', '0301', '03'),
('030102', 'Ancón', '0301', '03'),
('030201', 'Barranca', '0302', '03'),
('030202', 'Paramonga', '0302', '03'),

--LA LIBERTAD - DISTRITO
('040101', 'La Libertad', '0401', '04'),
('040102', 'Otuzco', '0401', '04'),
('040201', 'Trujillo', '0402', '04'),
('040202', 'La Esperanza', '0402', '04')
go


select * from DISTRITO
go

select * from USUARIO
go

--PROCEDIMIENTOS ALMACENADOS PARA USUARIO  
CREATE OR ALTER PROC sp_RegistrarUsuario(
@Nombres varchar(100),
@Apellidos varchar(100),
@Correo varchar(100),
@Clave varchar(100),
@Activo bit,
@Mensaje varchar(500) output,		--parametro de salida
@Resultado int output				--parametro de salida , devolvera el IdUsuario generado
)
as
begin
	SET	@Resultado = 0
	IF NOT EXISTS (SELECT * FROM USUARIO WHERE Correo = @Correo)
	begin
		insert into USUARIO(Nombres,Apellidos,Correo,Clave,Activo)values
		(@Nombres, @Apellidos, @Correo, @Clave, @Activo)

		SET @Resultado = SCOPE_IDENTITY()  --scope_identity  da el id generado (el ultimo de todos) despues de agregar el nuevo usuario
	end
	else
		set @Mensaje = 'El correo del usuario ya existe'
end
go

--Editar usuario
CREATE OR ALTER PROC sp_EditarUsuario(
@IdUsuario int,
@Nombres varchar(100),
@Apellidos varchar(100),
@Correo varchar(100),  --no vamos a editar la contraseña, esta ya es autogenerada / encriptada
@Activo bit,
@Mensaje varchar(500) output,		--parametro de salida
@Resultado bit output				--parametro de salida
)
as
begin
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM USUARIO WHERE CORREO = @Correo AND IdUsuario != @IdUsuario)
	begin		
		update top(1) USUARIO set
			Nombres = @Nombres,
			Apellidos = @Apellidos,
			Correo = @Correo,
			Activo = @Activo
			where IdUsuario = @IdUsuario
			set @Resultado = 1
	end
	else 
		set @Mensaje = 'El correo del usuario ya existe'
end


SELECT * FROM USUARIO
GO



--PROCEDIMIENTOS ALMACENADOS CATEGORIAS

CREATE OR ALTER PROC sp_RegistrarCategoria(
@Descripcion varchar(100),
@Activo bit,
@Mensaje varchar(500) output,		--parametro de salida
@Resultado int output				--parametro de salida , devolvera el IdUsuario generado
)
as
begin
	SET	@Resultado = 0
	IF NOT EXISTS (SELECT * FROM CATEGORIA WHERE Descripcion = @Descripcion)
	begin
		insert into CATEGORIA(Descripcion,Activo)values
		(@Descripcion, @Activo)

		SET @Resultado = SCOPE_IDENTITY()  --scope_identity  da el id generado (el ultimo de todos) despues de agregar el nuevo usuario
	end
	else
		set @Mensaje = 'La categoria ya existe'
end
go

--Editar categoria
CREATE OR ALTER PROC sp_EditarCategoria(
@IdCategoria int,
@Descripcion varchar(100),
@Activo bit,
@Mensaje varchar(500) output,		--parametro de salida
@Resultado bit output				--parametro de salida
)
as
begin
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM CATEGORIA WHERE Descripcion = @Descripcion AND IdCategoria != @IdCategoria)
	begin		
		update top(1) CATEGORIA set
			Descripcion = @Descripcion,
			Activo = @Activo
			where IdCategoria = @IdCategoria
			set @Resultado = 1
	end
	else 
		set @Mensaje = 'La categoria ya existe'
end
go

create or alter proc sp_EliminarCategoria(
@IdCategoria int,
@Mensaje varchar(500) output,		--parametro de salida
@Resultado bit output				--parametro de salida
)
as
begin
	SET @Resultado = 0
	--si no esta relacionado a un producto dicha categoria    -> el usuario tendria que eliminar un producto para que elimine dicha categoria
	IF NOT EXISTS (SELECT * FROM PRODUCTO p 
	inner join CATEGORIA c on c.IdCategoria = p.IdCategoria
	where p.IdCategoria = @IdCategoria)
	begin 
		delete top(1) from CATEGORIA where IdCategoria = @IdCategoria
		Set @Resultado = 1
	end
	else
		set @Mensaje = 'La categoria se encuentra relacionado a un producto'
end


Select * From CATEGORIA
go



--STORE PROCEDURE PARA MARCASS



CREATE OR ALTER PROC sp_RegistrarMarca(
@Descripcion varchar(100),
@Activo bit,
@Mensaje varchar(500) output,		--parametro de salida
@Resultado int output				--parametro de salida , devolvera el IdUsuario generado
)
as
begin
	SET	@Resultado = 0
	IF NOT EXISTS (SELECT * FROM MARCA WHERE Descripcion = @Descripcion)
	begin
		insert into MARCA(Descripcion,Activo)values
		(@Descripcion, @Activo)

		SET @Resultado = SCOPE_IDENTITY()  --scope_identity  da el id generado (el ultimo de todos) despues de agregar el nuevo usuario
	end
	else
		set @Mensaje = 'La marca ya existe'
end
go

--Editar categoria
CREATE OR ALTER PROC sp_EditarMarca(
@IdMarca int,
@Descripcion varchar(100),
@Activo bit,
@Mensaje varchar(500) output,		--parametro de salida
@Resultado bit output				--parametro de salida
)
as
begin
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM MARCA WHERE Descripcion = @Descripcion AND IdMarca != @IdMarca)
	begin		
		update top(1) MARCA set
			Descripcion = @Descripcion,
			Activo = @Activo
			where IdMarca = @IdMarca
			set @Resultado = 1
	end
	else 
		set @Mensaje = 'La marca ya existe'
end
go

create or alter proc sp_EliminarMarca(
@IdMarca int,
@Mensaje varchar(500) output,		--parametro de salida
@Resultado bit output				--parametro de salida
)
as
begin
	SET @Resultado = 0
	--si no esta relacionado a un producto dicha categoria    -> el usuario tendria que eliminar un producto para que elimine dicha categoria
	IF NOT EXISTS (SELECT * FROM PRODUCTO p 
	inner join Marca m on m.IdMarca = p.IdMarca
	where p.IdMarca = @IdMarca)
	begin 
		delete top(1) from MARCA where IdMarca = @IdMarca
		Set @Resultado = 1
	end
	else
		set @Mensaje = 'La marca se encuentra relacionado a un producto'
end


Select * From MARCA
go



--	STORE PROCEDUREEEEEE PRODUCTO
CREATE OR ALTER PROC sp_RegistrarProducto(
@Nombre varchar(100),
@Descripcion varchar(100),
@IdMarca varchar(100),
@IdCategoria varchar(100),
@Precio decimal (10,2),
@Stock int,
@Activo bit,
@Mensaje varchar(500) output,		--parametro de salida
@Resultado int output				--parametro de salida , devolvera el IdUsuario generado
)
as
begin
	SET	@Resultado = 0
	IF NOT EXISTS (SELECT * FROM PRODUCTO WHERE NOMBRE = @Nombre)
	begin
		insert into PRODUCTO(Nombre,Descripcion,IdMarca, IdCategoria,Precio,Stock,Activo)values
		(@Nombre,@Descripcion,@IdMarca,@IdCategoria,@Precio,@Stock, @Activo)

		SET @Resultado = SCOPE_IDENTITY()  --scope_identity  da el id generado (el ultimo de todos) despues de agregar el nuevo usuario
	end
	else
		set @Mensaje = 'El producto ya existe'
end
go

--Editar

CREATE OR ALTER PROC sp_EditarProducto(
@IdProducto int,
@Nombre varchar(100),
@Descripcion varchar(100),
@IdMarca varchar(100),
@IdCategoria varchar(100),
@Precio decimal (10,2),
@Stock int,
@Activo bit,
@Mensaje varchar(500) output,		--parametro de salida
@Resultado int output				--parametro de salida , devolvera el IdUsuario generado
)
as
begin
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM PRODUCTO WHERE Nombre = @Nombre AND IdProducto != @IdProducto)
	begin		
		update top(1) PRODUCTO set
			Nombre = @Nombre,
			Descripcion = @Descripcion,
			IdMarca= @IdMarca,
			IdCategoria = @IdCategoria,
			Precio= @Precio,
			Stock= @Stock,
			Activo = @Activo
			where IdProducto = @IdProducto
			set @Resultado = 1
	end
	else 
		set @Mensaje = 'El producto ya existe'
end
go


--eliminar producto
create or alter proc sp_EliminarProducto(
@IdProducto int,
@Mensaje varchar(500) output,		--parametro de salida
@Resultado bit output				--parametro de salida
)
as
begin
	SET @Resultado = 0
	If NOT EXISTS (Select * from DETALLE_VENTA dv
	inner join Producto p on p.IdProducto = dv.IdProducto
	WHERE p.IdProducto = @IdProducto)
	begin
		delete top(1) from PRODUCTO where IdProducto = @IdProducto
		SET @Resultado = 1
	end
	else
	set @Mensaje = 'El producto se encuentra relacionado a una venta'
end

select * from Producto
go


--LISTAR PRODUCTOS

SELECT P.IdProducto, p.Nombre, p.Descripcion,
m.IdMarca, m.Descripcion[Marcas],
c.IdCategoria, c.Descripcion[Categorias],
p.Precio, p.Stock, p.RutaImagen, p.NombreImagen, p.Activo
FROM PRODUCTO p
inner join Marca m on m.IdMarca = P.IdMarca
inner join CATEGORIA C on C.IdCategoria = P.IdCategoria


select * from USUARIO

select * from CATEGORIA

select * from marca
go

--Crear procedimientos almacenados para poder usar en el dashboard


Create or alter proc sp_ReporteDashboard
as
begin

select
	(select count(*) from CLIENTE) [TotalCliente],
	(select isnull(sum(cantidad),0) from DETALLE_VENTA) [TotalVenta],  --nos muestra la suma de cada cantidad , si es null, arroja 0
	(select count(*) from PRODUCTO) [TotalProducto]
end
go


--Ahora trabajaremos con las fechas para poder retornar en la lista del dashboard por fechas
--Usamos Paypal para hacer uso de la transacción
--Formato fecha CONVERT
--Unir nombre y apellido CONCAT

--Se crea el procedimiento almacenado
create or alter proc sp_ReporteVentas(
@fechaInicio varchar(10),
@fechaFin varchar(10),
@idTransaccion varchar(50)
)
as
begin
	set dateformat dmy; --day/month/year = dia/mes/año
	select CONVERT (char(10),v.FechaVenta,103)[FechaVenta],CONCAT( c.Nombres,' ',c.Apellidos)[Cliente],
	p.Nombre[Producto], p.Precio, dv.Cantidad, dv.Total, v.IdTransaccion from DETALLE_VENTA dv
	inner join PRODUCTO p on p.IdProducto = dv.IdProducto
	inner join Venta v on v.IdVenta = dv.IdVenta
	inner join Cliente c on c.IdCliente = v.IdCliente
	where CONVERT(date, v.FechaVenta) between @fechaInicio and @fechaFin
	and v.IdTransaccion = iif(@idTransaccion = '', v.IdTransaccion,@idTransaccion)
end

select * from USUARIO
go



--AHORA TOCA TRABAJAR CON EL MODULO PRESENTADO DE LA TIENDA, LA VISTA POR PARTE DEL CLIENTE
select * from CLIENTE
go


create or alter proc sp_RegistrarCliente(
	@Nombres varchar(100),
	@Apellidos varchar(100),
	@Correo varchar(100),
	@Clave varchar(100),
	@Mensaje varchar(500) output,
	@Resultado int output
)
as
begin
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM CLIENTE WHERE Correo = @Correo)
	begin
		insert into CLIENTE(Nombres, Apellidos, Correo, Clave, Reestablecer) VALUES
		(@Nombres, @Apellidos, @Correo, @Clave, 0)

		SET @Resultado = SCOPE_IDENTITY()
	end
	else
		set @Mensaje = 'El corre del usuario ya existe'
end

--Trabajamos con procedimiento almacenados para que nos liste los productos mediante la selección de categorias y marcas

declare @idCategoria int = 0
select DISTINCT m.IdMarca, m.Descripcion from Producto p
inner join Categoria c on c.IdCategoria = p.IdCategoria
inner Join Marca m on m.IdMarca = p.IdMarca and m.Activo = 1
where c.IdCategoria = iif(@idCategoria = 0, c.IdCategoria, @idCategoria)
go

--Procedimientos Almacenados Carrito

create or alter proc sp_ExisteCarrito(
	@IdCliente int,
	@IdProducto int,
	@Resultado bit output
)
as
begin
	if exists(select * from CARRITO where IdCliente = @IdCliente and IdProducto =@IdProducto)
		set @Resultado = 1
	else 
		set @Resultado = 0
end
go

--el procedimiento almacenado más largo que he echo jaja

create or alter proc sp_OperacionCarrito(
	@IdCliente int,
	@IdProducto int,
	@Sumar bit,
	@Mensaje varchar(500) output,
	@Resultado bit output
)
as
begin
	set @Resultado = 1
	set @Mensaje = ''

	declare @existecarrito bit = iif (exists(select * from CARRITO where IdCliente = @IdCliente and IdProducto =@IdProducto),1,0)
	declare @stockproducto int = (select stock from PRODUCTO where IdProducto = @IdProducto)

	BEGIN TRY -- TRABAJAMOS CON TRANSACTION PORQUE OPERAMOS CON VALORES EN TIEMPO REAL
		BEGIN TRANSACTION OPERACION --LOGICA DE LA SUMA DE ARTICULOS SELECCIONADOS AL CARRITO DE COMPRAS

		if(@Sumar = 1)
		begin
			
			if(@stockproducto >0)
			begin
				if(@existecarrito = 1)
					update CARRITO set Cantidad = Cantidad + 1 where IdCliente = @IdCliente and IdProducto =@IdProducto
				else
					insert into CARRITO(IdCliente, IdProducto, Cantidad) values (@IdCliente, @IdProducto, 1)

				update PRODUCTO set Stock = Stock - 1 where IdProducto =@IdProducto
			end
			else
			begin
				set @Resultado = 0
				set @Mensaje = 'El producto no cuenta con stock disponible'
			end
		end
		else
		begin
			update CARRITO set Cantidad = Cantidad - 1 where IdCliente = @IdCliente and IdProducto =@IdProducto
			update PRODUCTO set Stock = Stock + 1 where IdProducto =@IdProducto
		end
		Commit Transaction OPERACION
	

	END TRY
	BEGIN CATCH
		set @Resultado = 0
		set @Mensaje = ERROR_MESSAGE()
		Rollback Transaction OPERACION
	END CATCH
end
go


select * from cliente
go

--creamos funcion que retornara el id cliente y la lógica para que retorne los datos requeridos
create function fn_obtenerCarritoCliente(
	@idCliente int
)
returns table
as
return(
	select p.IdProducto, m.Descripcion[DesMarca], p.Nombre, p.Precio, c.Cantidad,p.RutaImagen,p.NombreImagen
	from Carrito c
	inner join Producto p on p.IdProducto = C.IdProducto
	inner join Marca m on m.IdMarca = p.IdMarca
	where c.IdCliente = @idCliente
)
go

select * from fn_obtenerCarritoCliente(2)
go


--crear proc almacenado de eliminarCarrito

create or alter proc sp_EliminarCarrito(
@IdCliente int,
@IdProducto int,
@Resultado bit output
)
as
begin
	set @Resultado = 1
	declare @cantidadproducto int = (select Cantidad from CARRITO where IdCliente = @IdCliente AND IdProducto = @IdProducto)

	begin try

		begin transaction OPERACION

		UPDATE PRODUCTO SET STOCK = STOCK +@cantidadproducto WHERE IdProducto = @IdProducto
		DELETE TOP(1) FROM CARRITO WHERE IdCliente = @IdCliente AND IdProducto = @IdProducto

		COMMIT TRANSACTION OPERACION
	END TRY

	BEGIN CATCH
		SET @Resultado = 0
		ROLLBACK TRANSACTION OPERACION
	END CATCH
END
go


select DISTINCT * from DEPARTAMENTO
go

select DISTINCT * from PROVINCIA where IdDepartamento = '01'
GO

select DISTINCT * from DISTRITO where IdProvincia = '0101' and IdDepartamento = '01'
go


--UNA VENTA SIGNIFICA UNA LSITA ESTRUCTURA DE PRODUCTOS

CREATE TYPE [dbo].[EDetalle_Venta] AS TABLE(
	[IdProducto] int null,
	[Cantidad]int null,
	[Total] decimal(18,2) null
)
go

create or alter procedure usp_RegistrarVenta(
	@IdCliente int,
	@TotalProducto int,
	@MontoTotal decimal(18,2),
	@Contacto varchar(100),
	@IdDistrito varchar(6),
	@Telefono varchar(10),
	@Direccion varchar(100),
	@IdTransaccion varchar(50),
	@DetalleVenta [EDetalle_Venta] READONLY,
	@Resultado bit output,
	@Mensaje varchar(530) output
)
as
begin	
	begin try
		declare @idVenta int= 0
		set @Resultado = 1
		set @Mensaje = ''

		begin transaction registro
		insert into VENTA(IdCliente,TotalProducto,MontoTotal,Contacto,IdDistrito,Telefono,Direccion,IdTransaccion)
		values(@IdCliente,@TotalProducto,@MontoTotal,@Contacto,@IdDistrito,@Telefono,@Direccion,@IdTransaccion)

		set @idVenta = SCOPE_IDENTITY()

		insert into DETALLE_VENTA(IdVenta,IdProducto,Cantidad, Total)
		select @idVenta,IdProducto,Cantidad,Total from @DetalleVenta

		Delete from CARRITO WHERE IdCliente = @IdCliente

		commit transaction registro
	end try

	begin catch
		set @Resultado = 0
		set @Mensaje = ERROR_MESSAGE()
		rollback transaction registro
	end catch
end


--Crear la consulta que nos permita ver toda las compras del cliente
create function fn_ListarCompra(
	@idCliente int
)
returns table
as
return
(
	Select p.RutaImagen, p.NombreImagen, p.Nombre, p.Precio,dv.Cantidad,dv.Total,v.IdTransaccion from DETALLE_VENTA dv
	inner join PRODUCTO p on p.IdProducto = dv.IdProducto
	inner join venta v on v.IdVenta = dv.IdVenta
	where v.IdCliente = @idCliente
)
go

