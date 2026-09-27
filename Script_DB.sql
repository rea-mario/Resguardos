
Use master;
GO

Create DATABASE ResguardosDb;
GO
USE  ResguardosDb;
GO

CREATE TABLE [dbo].[Proveedores](
	[ProveedorId] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](200) NOT NULL,
	[NombreContacto] [nvarchar](150) NULL,
	[Correo] [nvarchar](150) NULL,
	[Telefono] [nvarchar](50) NULL,
	[ProporcionaCompra] [bit] NOT NULL,
	[ProporcionaRenta] [bit] NOT NULL,
	[ProporcionaMantenimiento] [bit] NOT NULL,
	[RFC] [nvarchar](20) NOT NULL,
	[Activo] [bit] NOT NULL,
	[FechaCreacion] [datetime2](7) NOT NULL
) ON [PRIMARY]

GO
CREATE TABLE [dbo].[Activos](
	[ActivoId] [int] IDENTITY(1,1) NOT NULL,
	[AssetCode] [nvarchar](50) NOT NULL,
	[SerialNumber] [nvarchar](100) NULL,
	[Category] [nvarchar](100) NOT NULL,
	[Brand] [nvarchar](100) NULL,
	[Model] [nvarchar](100) NULL,
	[OwnershipType] [nvarchar](30) NOT NULL,
	[SupplierId] [int] NULL,
	[Status] [nvarchar](30) NOT NULL,
	[CurrentLocation] [nvarchar](200) NULL,
	[PurchaseDate] [date] NULL,
	[RentalEndDate] [date] NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL
) ON [PRIMARY]

GO
CREATE TABLE [dbo].[Asignaciones](
	[AsignacionId] [bigint] IDENTITY(1,1) NOT NULL,
	[ActivoId] [int] NOT NULL,
	[EmpleadoId] [int] NOT NULL,
	[FechaAsignacion] [datetime2](7) NOT NULL,
	[FechaDevolucion] [datetime2](7) NULL,
	[CondicionDevolucion] [nvarchar](200) NULL,
	[AsignadoPor] [int] NOT NULL,
	[DevueltoPor] [int] NULL,
	[Observaciones] [nvarchar](500) NULL
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[Empleados](
	[EmpleadoId] [int] IDENTITY(1,1) NOT NULL,
	[NumeroEmpleado] [nvarchar](50) NOT NULL,
	[Nombre] [nvarchar](150) NOT NULL,
	[Correo] [nvarchar](150) NOT NULL,
	[Activo] [bit] NOT NULL,
	[FechaCreacion] [datetime2](7) NOT NULL
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[MovimientosActivos](
	[MovimientoActivoId] [bigint] IDENTITY(1,1) NOT NULL,
	[ActivoId] [int] NOT NULL,
	[UsuarioId] [int] NOT NULL,
	[TipoMovimiento] [nvarchar](50) NOT NULL,
	[FechaMovimiento] [datetime2](7) NOT NULL,
	[EstadoAnterior] [nvarchar](30) NULL,
	[EstadoNuevo] [nvarchar](30) NULL,
	[UbicacionAnterior] [nvarchar](200) NULL,
	[UbicacionNueva] [nvarchar](200) NULL,
	[Observaciones] [nvarchar](500) NULL
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[Usuarios](
	[UsuarioId] [int] IDENTITY(1,1) NOT NULL,
	[Usuario] [nvarchar](100) NOT NULL,
	[HashContrasena] [nvarchar](500) NOT NULL,
	[Rol] [nvarchar](30) NOT NULL,
	[Activo] [bit] NOT NULL,
	[FechaCreacion] [datetime2](7) NOT NULL,
	[IntentosFallidos] [int] NOT NULL,
	[BloqueadoHasta] [datetime2](7) NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Proveedores] ADD PRIMARY KEY CLUSTERED 
(
	[ProveedorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
ALTER TABLE [dbo].[Proveedores] ADD  CONSTRAINT [UQ_Proveedores_Nombre] UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Proveedores] ADD  DEFAULT ((0)) FOR [ProporcionaCompra]
GO
ALTER TABLE [dbo].[Proveedores] ADD  DEFAULT ((0)) FOR [ProporcionaRenta]
GO
ALTER TABLE [dbo].[Proveedores] ADD  DEFAULT ((0)) FOR [ProporcionaMantenimiento]
GO
ALTER TABLE [dbo].[Proveedores] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Proveedores] ADD  DEFAULT (sysdatetime()) FOR [FechaCreacion]
GO

ALTER TABLE [dbo].[Activos] ADD PRIMARY KEY CLUSTERED 
(
	[ActivoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
ALTER TABLE [dbo].[Activos] ADD  CONSTRAINT [UQ_Activos_CodigoActivo] UNIQUE NONCLUSTERED 
(
	[AssetCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Activos_NumeroSerie] ON [dbo].[Activos]
(
	[SerialNumber] ASC
)
WHERE ([SerialNumber] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Activos] ADD  DEFAULT (sysdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Activos]  WITH CHECK ADD  CONSTRAINT [FK_Activos_Proveedores] FOREIGN KEY([SupplierId])
REFERENCES [dbo].[Proveedores] ([ProveedorId])
GO
ALTER TABLE [dbo].[Activos] CHECK CONSTRAINT [FK_Activos_Proveedores]
GO
ALTER TABLE [dbo].[Activos]  WITH CHECK ADD  CONSTRAINT [CK_Activos_Status] CHECK  (([Status]='Retirado' OR [Status]='Mantenimiento' OR [Status]='Asignado' OR [Status]='Disponible'))
GO
ALTER TABLE [dbo].[Activos] CHECK CONSTRAINT [CK_Activos_Status]
GO
ALTER TABLE [dbo].[Usuarios] ADD PRIMARY KEY CLUSTERED 
(
	[UsuarioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
ALTER TABLE [dbo].[Usuarios] ADD  CONSTRAINT [UQ_Usuarios_Usuario] UNIQUE NONCLUSTERED 
(
	[Usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Usuarios] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Usuarios] ADD  DEFAULT (sysdatetime()) FOR [FechaCreacion]
GO
ALTER TABLE [dbo].[Usuarios] ADD  CONSTRAINT [DF_Usuarios_IntentosFallidos]  DEFAULT ((0)) FOR [IntentosFallidos]
GO
ALTER TABLE [dbo].[Usuarios]  WITH CHECK ADD  CONSTRAINT [CK_Usuarios_Rol] CHECK  (([Rol]='Operador' OR [Rol]='Administrador'))
GO
ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [CK_Usuarios_Rol]
GO
ALTER TABLE [dbo].[Empleados] ADD PRIMARY KEY CLUSTERED 
(
	[EmpleadoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
ALTER TABLE [dbo].[Empleados] ADD  CONSTRAINT [UQ_Empleados_Correo] UNIQUE NONCLUSTERED 
(
	[Correo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
ALTER TABLE [dbo].[Empleados] ADD  CONSTRAINT [UQ_Empleados_NumeroEmpleado] UNIQUE NONCLUSTERED 
(
	[NumeroEmpleado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Empleados] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Empleados] ADD  DEFAULT (sysdatetime()) FOR [FechaCreacion]
GO
ALTER TABLE [dbo].[Asignaciones] ADD PRIMARY KEY CLUSTERED 
(
	[AsignacionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Asignaciones_ActivoActivo] ON [dbo].[Asignaciones]
(
	[ActivoId] ASC
)
WHERE ([FechaDevolucion] IS NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Asignaciones] ADD  DEFAULT (sysdatetime()) FOR [FechaAsignacion]
GO
ALTER TABLE [dbo].[Asignaciones]  WITH CHECK ADD  CONSTRAINT [FK_Asignaciones_Activos] FOREIGN KEY([ActivoId])
REFERENCES [dbo].[Activos] ([ActivoId])
GO
ALTER TABLE [dbo].[Asignaciones] CHECK CONSTRAINT [FK_Asignaciones_Activos]
GO
ALTER TABLE [dbo].[Asignaciones]  WITH CHECK ADD  CONSTRAINT [FK_Asignaciones_Empleados] FOREIGN KEY([EmpleadoId])
REFERENCES [dbo].[Empleados] ([EmpleadoId])
GO
ALTER TABLE [dbo].[Asignaciones] CHECK CONSTRAINT [FK_Asignaciones_Empleados]
GO
ALTER TABLE [dbo].[Asignaciones]  WITH CHECK ADD  CONSTRAINT [FK_Asignaciones_UsuarioAsignacion] FOREIGN KEY([AsignadoPor])
REFERENCES [dbo].[Usuarios] ([UsuarioId])
GO
ALTER TABLE [dbo].[Asignaciones] CHECK CONSTRAINT [FK_Asignaciones_UsuarioAsignacion]
GO
ALTER TABLE [dbo].[Asignaciones]  WITH CHECK ADD  CONSTRAINT [FK_Asignaciones_UsuarioDevolucion] FOREIGN KEY([DevueltoPor])
REFERENCES [dbo].[Usuarios] ([UsuarioId])
GO
ALTER TABLE [dbo].[Asignaciones] CHECK CONSTRAINT [FK_Asignaciones_UsuarioDevolucion]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Inserta Usuario
insert into Usuarios
select N'admin',N'$2a$11$kLNca.kdP3kadGIqYs8Gzew9qVAagMXzUm/Wk9ekesDQAXdLKQxXS',N'Administrador',1,'2026-09-27 00:31:57.3600000',0,NULL UNION ALL
select N'operador',N'$2a$11$XvrSQU.054sNg8Zid/f6nOQgvqvij8E7vwkNhnbIDkVtk0.ni6XaO',N'Operador',1,'2026-09-27 00:31:57.3600000',0,NULL;
GO
--Inserta Store
CREATE   PROCEDURE [dbo].[SP_Activos_Consultar]
(
    @Id INT = NULL,
    @CodigoActivo NVARCHAR(50) = NULL,
    @Categoria NVARCHAR(100) = NULL,
    @Estado NVARCHAR(30) = NULL,
    @TipoPropiedad NVARCHAR(30) = NULL,
    @IdProveedor INT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    
        SELECT
        a.ActivoId,
        a.AssetCode,
        a.SerialNumber,
        a.Category,
        a.Brand,
        a.Model,
        a.OwnershipType,
        a.SupplierId,
        p.Nombre AS Supplier,
        a.[Status],
        a.CurrentLocation,
        a.PurchaseDate,
        a.RentalEndDate,
        a.CreatedAt,
        a.UpdatedAt

    FROM Activos a

    LEFT JOIN Proveedores p
        ON p.ProveedorId = a.SupplierId

    WHERE
        (@Id IS NULL OR a.ActivoId = @Id)

AND (
            @CodigoActivo IS NULL
            OR a.AssetCode = @CodigoActivo
        )

        AND (
            @Categoria IS NULL
            OR a.Category = @Categoria
        )

        AND (
            @Estado IS NULL
            OR a.[Status] = @Estado
        )

        AND (
            @TipoPropiedad IS NULL
            OR a.OwnershipType = @TipoPropiedad
        )

        AND (
            @IdProveedor IS NULL
            OR a.SupplierId = @IdProveedor
        )

    ORDER BY
        a.CreatedAt DESC;
END;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_Activos_Crear]
    (
    @CodigoActivo NVARCHAR(50),
    @NumeroSerie NVARCHAR(100) = NULL,
    @Categoria NVARCHAR(100),
    @Marca NVARCHAR(100) = NULL,
    @Modelo NVARCHAR(100) = NULL,
    @TipoPropiedad NVARCHAR(30),
    @IdProveedor INT = NULL,
    @Estado NVARCHAR(30),
    @UbicacionActual NVARCHAR(200) = NULL,
    @FechaCompra DATE = NULL,
    @FechaFinRenta DATE = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        -- Validar código duplicado
        IF EXISTS (
            SELECT 1
    FROM Activos
    WHERE AssetCode = @CodigoActivo
        )
        BEGIN
            THROW 50020, 'El código del activo ya existe.', 1;
        END;

-- Validar número de serie duplicado
        IF @NumeroSerie IS NOT NULL
        AND EXISTS (
                SELECT 1
        FROM Activos
        WHERE SerialNumber = @NumeroSerie
           )
        BEGIN
            THROW 50029,
                'El número de serie ya está registrado.', 1;
        END;

    -- Validar propiedad
    IF @TipoPropiedad NOT IN ('Propio', 'Arrendado')
        BEGIN
    THROW 50030,
                'El tipo de propiedad no es válido.', 1;
END;

        IF @TipoPropiedad = 'Arrendado'
    AND @IdProveedor IS NULL
        BEGIN
            THROW 50001,
                'Un activo arrendado debe tener proveedor.', 1;
        END;

        IF @Estado NOT IN (
                    'Disponible',
                    'Asignado',
                    'Mantenimiento',
                    'Retirado'
                          )
        BEGIN
            THROW 50001, 'El estado del activo no es válido.', 1;
        END;

        INSERT INTO Activos
    (
    AssetCode,
    SerialNumber,
    Category,
    Brand,
    Model,
    OwnershipType,
    SupplierId,
    [Status],
    CurrentLocation,
    PurchaseDate,
    RentalEndDate,
    CreatedAt,
    UpdatedAt
    )
VALUES
    (
        @CodigoActivo,
        @NumeroSerie,
        @Categoria,
        @Marca,
        @Modelo,
        @TipoPropiedad,
        @IdProveedor,
        @Estado,
        @UbicacionActual,
        @FechaCompra,
        @FechaFinRenta,
        GETDATE(),
        NULL
        );

        SELECT
    SCOPE_IDENTITY() AS Id;

    END
    TRY
    BEGIN CATCH
    THROW;
    END CATCH
END;
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_Asignacion_Crear]
    @ActivoId INT,
    @EmpleadoId INT,
    @AsignadoPor INT,
    @Observaciones NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE
            @EstadoActual NVARCHAR(30),
            @UbicacionActual NVARCHAR(200),
            @AsignacionId BIGINT;

        -- Bloquea el activo para evitar asignaciones simultáneas.
        SELECT
            @EstadoActual = [Status],
            @UbicacionActual = CurrentLocation
        FROM Activos WITH (UPDLOCK, HOLDLOCK)
        WHERE ActivoId = @ActivoId;

        IF @EstadoActual IS NULL
        BEGIN
            THROW 50020, 'El activo no existe.', 1;
        END;

        IF @EstadoActual <> 'Disponible'
        BEGIN
            THROW 50021,
                'El activo no está disponible para asignación.', 1;
        END;

        IF NOT EXISTS (
            SELECT 1
            FROM Empleados
            WHERE EmpleadoId = @EmpleadoId
              AND Activo = 1
        )
        BEGIN
            THROW 50022,
                'El empleado no existe o está inactivo.', 1;
        END;

        IF NOT EXISTS (
            SELECT 1
            FROM Usuarios
            WHERE UsuarioId = @AsignadoPor
              AND Activo = 1
        )
        BEGIN
            THROW 50023,
                'El usuario que asigna no existe o está inactivo.', 1;
        END;

        IF EXISTS (
            SELECT 1
            FROM Asignaciones
            WHERE ActivoId = @ActivoId
              AND FechaDevolucion IS NULL
        )
        BEGIN
            THROW 50024,
                'El activo ya tiene una asignación activa.', 1;
        END;

        INSERT INTO Asignaciones
        (
            ActivoId,
            EmpleadoId,
            FechaAsignacion,
            AsignadoPor,
            Observaciones
        )
        VALUES
        (
            @ActivoId,
            @EmpleadoId,
            SYSDATETIME(),
            @AsignadoPor,
            @Observaciones
        );

        SET @AsignacionId =
            CONVERT(BIGINT, SCOPE_IDENTITY());

        UPDATE Activos
        SET
            [Status] = 'Asignado',
            UpdatedAt = SYSDATETIME()
        WHERE ActivoId = @ActivoId;

        INSERT INTO MovimientosActivos
        (
            ActivoId,
            UsuarioId,
            TipoMovimiento,
            FechaMovimiento,
            EstadoAnterior,
            EstadoNuevo,
            UbicacionAnterior,
            UbicacionNueva,
            Observaciones
        )
        VALUES
        (
            @ActivoId,
            @AsignadoPor,
            'ASIGNACION',
            SYSDATETIME(),
            'Disponible',
            'Asignado',
            @UbicacionActual,
            @UbicacionActual,
            @Observaciones
        );

        COMMIT TRANSACTION;

        SELECT @AsignacionId AS AsignacionId;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH;
END;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_Asignacion_Devolver]
    @ActivoId INT,
    @DevueltoPor INT,
    @CondicionDevolucion NVARCHAR(200),
    @Observaciones NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE
            @AsignacionId BIGINT,
            @EstadoActual NVARCHAR(30),
            @UbicacionActual NVARCHAR(200);

        SELECT
            @EstadoActual = [Status],
            @UbicacionActual = CurrentLocation
        FROM Activos WITH (UPDLOCK, HOLDLOCK)
        WHERE ActivoId = @ActivoId;

        IF @EstadoActual IS NULL
        BEGIN
            THROW 50025, 'El activo no existe.', 1;
        END;

        IF @EstadoActual <> 'Asignado'
        BEGIN
            THROW 50026,
                'El activo no tiene estado Asignado.', 1;
        END;

        IF NOT EXISTS (
            SELECT 1
            FROM Usuarios
            WHERE UsuarioId = @DevueltoPor
              AND Activo = 1
        )
        BEGIN
            THROW 50027,
                'El usuario que registra la devolución no es válido.', 1;
        END;

        SELECT @AsignacionId = AsignacionId
        FROM Asignaciones WITH (UPDLOCK, HOLDLOCK)
        WHERE ActivoId = @ActivoId
          AND FechaDevolucion IS NULL;

        IF @AsignacionId IS NULL
        BEGIN
            THROW 50028,
                'No existe una asignación activa para este activo.', 1;
        END;

        UPDATE Asignaciones
        SET
            FechaDevolucion = SYSDATETIME(),
            CondicionDevolucion = @CondicionDevolucion,
            DevueltoPor = @DevueltoPor,
            Observaciones =
                CASE
                    WHEN @Observaciones IS NULL
                        THEN Observaciones
                    WHEN Observaciones IS NULL
                        THEN @Observaciones
                    ELSE Observaciones + N' | ' + @Observaciones
                END
        WHERE AsignacionId = @AsignacionId;

        UPDATE Activos
        SET
            [Status] = 'Disponible',
            UpdatedAt = SYSDATETIME()
        WHERE ActivoId = @ActivoId;

        INSERT INTO MovimientosActivos
        (
            ActivoId,
            UsuarioId,
            TipoMovimiento,
            FechaMovimiento,
            EstadoAnterior,
            EstadoNuevo,
            UbicacionAnterior,
            UbicacionNueva,
            Observaciones
        )
        VALUES
        (
            @ActivoId,
            @DevueltoPor,
            'DEVOLUCION',
            SYSDATETIME(),
            'Asignado',
            'Disponible',
            @UbicacionActual,
            @UbicacionActual,
            @CondicionDevolucion +
                CASE
                    WHEN @Observaciones IS NOT NULL
                        THEN N' | ' + @Observaciones
                    ELSE N''
                END
        );

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH;
END;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_Asignacion_ObtenerPorActivo]
    @ActivoId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        a.AsignacionId,
        a.ActivoId,
        ac.AssetCode[CodigoActivo],
        a.EmpleadoId,
        e.NumeroEmpleado,
        e.Nombre AS NombreEmpleado,
        a.FechaAsignacion,
        a.FechaDevolucion,
        a.CondicionDevolucion,
        a.AsignadoPor,
        a.DevueltoPor,
        a.Observaciones
    FROM Asignaciones a
    INNER JOIN Activos ac
        ON ac.ActivoId = a.ActivoId
    INNER JOIN Empleados e
        ON e.EmpleadoId = a.EmpleadoId
    WHERE a.ActivoId = @ActivoId
    ORDER BY a.FechaAsignacion DESC;
END;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_Asignacion_ObtenerPorEmpleado]
    @EmpleadoId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        a.AsignacionId,
        a.ActivoId,
        ac.AssetCode [CodigoActivo],
        a.EmpleadoId,
        e.NumeroEmpleado,
        e.Nombre AS NombreEmpleado,
        a.FechaAsignacion,
        a.FechaDevolucion,
        a.CondicionDevolucion,
        a.AsignadoPor,
        a.DevueltoPor,
        a.Observaciones
    FROM Asignaciones a
    INNER JOIN Activos ac
        ON ac.ActivoId = a.ActivoId
    INNER JOIN Empleados e
        ON e.EmpleadoId = a.EmpleadoId
    WHERE a.EmpleadoId = @EmpleadoId
    ORDER BY a.FechaAsignacion DESC;
END;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_Empleado_Crear]
    @NumeroEmpleado NVARCHAR(50),
    @Nombre NVARCHAR(150),
    @Correo NVARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM Empleados
        WHERE NumeroEmpleado = @NumeroEmpleado
    )
    BEGIN
        THROW 50010,
            'El numero de empleado ya existe.', 1;
    END;

    IF EXISTS (
        SELECT 1
        FROM Empleados
        WHERE Correo = @Correo
    )
    BEGIN
        THROW 50011,
            'El correo ya existe.', 1;
    END;

    INSERT INTO Empleados
    (
        NumeroEmpleado,
        Nombre,
        Correo,
        Activo,
        FechaCreacion
    )
    VALUES
    (
        @NumeroEmpleado,
        @Nombre,
        @Correo,
        1,
        SYSDATETIME()
    );

    SELECT CONVERT(INT, SCOPE_IDENTITY()) AS EmpleadoId;
END;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_Empleado_ObtenerPorId]
    @EmpleadoId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        EmpleadoId,
        NumeroEmpleado,
        Nombre,
        Correo,
        Activo,
        FechaCreacion
    FROM Empleados
    WHERE EmpleadoId = @EmpleadoId;
END;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_Empleado_ObtenerTodos]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        EmpleadoId,
        NumeroEmpleado,
        Nombre,
        Correo,
        Activo,
        FechaCreacion
    FROM Empleados
    ORDER BY Nombre;
END;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_Proveedores_Consultar]
    (
    @ProveedorId INT = NULL,
    @Nombre NVARCHAR(200) = NULL,
    @RFC NVARCHAR(20) = NULL,
    @Activo BIT = NULL,
    @ProporcionaCompra BIT = NULL,
    @ProporcionaRenta BIT = NULL,
    @ProporcionaMantenimiento BIT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ProveedorId,
        Nombre,
        NombreContacto,
        Correo,
        Telefono,
        ProporcionaCompra,
        ProporcionaRenta,
        ProporcionaMantenimiento,
        RFC,
        Activo,
        FechaCreacion
    FROM Proveedores
    WHERE
        (@ProveedorId IS NULL
        OR ProveedorId = @ProveedorId)

        AND (@Nombre IS NULL
        OR Nombre LIKE '%' + @Nombre + '%')

        AND (@RFC IS NULL
        OR RFC = @RFC)

        AND (@Activo IS NULL
        OR Activo = @Activo)

        AND (@ProporcionaCompra IS NULL
        OR ProporcionaCompra = @ProporcionaCompra)

        AND (@ProporcionaRenta IS NULL
        OR ProporcionaRenta = @ProporcionaRenta)

        AND (@ProporcionaMantenimiento IS NULL
        OR ProporcionaMantenimiento = @ProporcionaMantenimiento)

    ORDER BY Nombre;
END;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_Proveedores_Crear]
    (
    @Nombre NVARCHAR(200),
    @NombreContacto NVARCHAR(150) = NULL,
    @Correo NVARCHAR(150) = NULL,
    @Telefono NVARCHAR(50) = NULL,
    @ProporcionaCompra BIT = 0,
    @ProporcionaRenta BIT = 0,
    @ProporcionaMantenimiento BIT = 0,
    @RFC NVARCHAR(20)
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY -- Validar nombre 
    IF NULLIF(LTRIM(RTRIM(@Nombre)), '') IS NULL 
    BEGIN 
    THROW 50030, 'El nombre del proveedor es obligatorio.', 1; END; 
    -- Validar RFC 
    IF NULLIF(LTRIM(RTRIM(@RFC)), '') IS NULL 
    BEGIN 
    THROW 50031, 'El RFC del proveedor es obligatorio.', 1; END;
    -- Validar nombre duplicado 
    IF EXISTS ( SELECT 1
    FROM Proveedores
    WHERE Nombre = LTRIM(RTRIM(@Nombre)) ) 
    BEGIN
    THROW 50032, 'El proveedor ya existe.', 1;
END;
-- Validar RFC duplicado 
IF EXISTS ( SELECT 1
FROM Proveedores
WHERE RFC = LTRIM(RTRIM(@RFC)) ) 
    BEGIN
THROW 50033, 'El RFC del proveedor ya existe.', 1;
END;
INSERT INTO Proveedores
    ( Nombre, NombreContacto, Correo, Telefono, ProporcionaCompra, ProporcionaRenta, ProporcionaMantenimiento, RFC )
VALUES
    (
        LTRIM(RTRIM(@Nombre)),
        NULLIF(LTRIM(RTRIM(@NombreContacto)), ''),
        NULLIF(LTRIM(RTRIM(@Correo)), ''),
        NULLIF(LTRIM(RTRIM(@Telefono)), ''),
        @ProporcionaCompra,
        @ProporcionaRenta,
        @ProporcionaMantenimiento,
        LTRIM(RTRIM(@RFC)) 
    );

SELECT CAST(SCOPE_IDENTITY() AS INT) AS ProveedorId;
END 
        
        TRY 
        BEGIN
         CATCH
THROW;
END 
         CATCH
END;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_Usuario_Login]
    @Usuario NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

UPDATE dbo.Usuarios
SET
    IntentosFallidos = 0,
    BloqueadoHasta = NULL
WHERE Usuario = @Usuario
  AND BloqueadoHasta IS NOT NULL
  AND BloqueadoHasta <= GETDATE();
  
    SELECT
        UsuarioId,
        Usuario,
        HashContrasena,
        Rol,
        Activo,
        IntentosFallidos,
        BloqueadoHasta
    FROM Usuarios
    WHERE Usuario = @Usuario;
END;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_Usuario_RegistrarIntentoFallido]
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Intentos INT;
    DECLARE @BloqueadoHasta DATETIME;

    BEGIN TRANSACTION;

    UPDATE dbo.Usuarios 
    SET
        IntentosFallidos =
            CASE
                WHEN BloqueadoHasta IS NOT NULL
                     AND BloqueadoHasta <= GETDATE()
                THEN 1
                ELSE IntentosFallidos + 1
            END,
        BloqueadoHasta =
            CASE
                WHEN BloqueadoHasta IS NOT NULL
                     AND BloqueadoHasta > GETDATE()
                THEN BloqueadoHasta
                WHEN
                    CASE
                        WHEN BloqueadoHasta IS NOT NULL
                             AND BloqueadoHasta <= GETDATE()
                        THEN 1
                        ELSE IntentosFallidos + 1
                    END >= 5
                THEN DATEADD(MINUTE, 15, GETDATE())
                ELSE NULL
            END
    WHERE UsuarioId = @UsuarioId
      AND Activo = 1;

    SELECT
        @Intentos = IntentosFallidos,
        @BloqueadoHasta = BloqueadoHasta
    FROM dbo.Usuarios
    WHERE UsuarioId = @UsuarioId;

    COMMIT TRANSACTION;

    SELECT
        @Intentos AS IntentosFallidos,
        @BloqueadoHasta AS BloqueadoHasta;
END;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_Usuario_RestablecerIntentos]
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Usuarios
    SET
        IntentosFallidos = 0,
        BloqueadoHasta = NULL
    WHERE UsuarioId = @UsuarioId;
END;
GO
