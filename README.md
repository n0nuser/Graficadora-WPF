# Nota final : 

# Enunciado general de la práctica (2020)

Se desarrollará una aplicación en entorno WPF Microsoft Windows en C# con funcionalidad tipo “hoja de cálculo” muy básica. Deberá permitir crear, modificar y eliminar colecciones de datos para su representación gráfica. Se establecen las siguientes consideraciones:

- Las colecciones de datos (u hojas) son listas de pares de valores (coordenada X y coordenada Y). El usuario debe poder crear una colección y añadir, modificar y eliminar pares de valores sobre la misma.
- La aplicación permitirá al usuario realizar la representación gráfica de los datos contenidos en la hoja, permitiendo elegir, al menos, entre dos tipos de gráficas: gráfica de barras y polilínea.

La aplicación constará, al menos, de dos ventanas:

- En una ventana, LA PRINCIPAL, se visualizará la representación gráfica de los datos de la hoja.
- Un cuadro de diálogo o ventana secundaria que posibilite al usuario la gestión de los datos. Para gestionar los puntos de una hoja se utilizará una tabla, con tantas filas como pares de puntos y, obviamente, dos columnas, una para cada coordenada. El usuario deberá poder añadir nuevos pares de valores y eliminar o modificar pares de valores existentes.

La aplicación deberá permitir, como mínimo, dos formas de introducción de las coordenadas de los puntos:

- Introducción manual de ambos valores de coordenadas, X e Y
- Generación automática a partir de la indicación de un rango para los valores de las coordenadas X y una expresión polinómica configurable para la obtención de los valores de las coordenadas Y.

Una vez representada la gráfica en la VENTANA PRINCIPAL, se deberá permitir la realización de la selección mediante ratón de una porción rectangular de la misma y el purgado de los puntos de la colección de modo que solo permanezcan los incluidos en el área seleccionada.

Estos son los requisitos mínimos que debe cumplir la aplicación desarrollada, y constituyen el mínimo para conseguir la calificación de aprobado en la misma.

Adicionalmente se propone al alumno una serie de ideas que constituyen mejoras y que, lógicamente, supondrán una mejora en la calificación obtenida: 

- Configuración de los tipos de gráficas. Se pueden añadir más tipos de representación gráfica o bien configuración fina de la representación gráfica (configuración del grosor de línea, colores a utilizar en línea y relleno, tipo de relleno, etc).
- Gestión fina de la tabla permitiendo, por ejemplo, la modificación de datos sobre la misma tabla, o bien la posibilidad de reordenar los pares de valores.
- Los requisitos mínimos hablan de añadir solo por el final. Como mejora se podría permitir insertar un par de valores en una posición cualquiera de la hoja.

Por supuesto, el alumno puede incorporar cuantas mejoras desee, lo que repercutirá positivamente en la calificación obtenida. Se sugiere que dichas mejoras estén relacionadas con la utilización de controles visuales no estudiados en clase.

## Valoración de las prácticas

El enunciado propuesto es bastante concreto, en cuanto a las ventanas a mostrar, pero se deja abierto el detalle, de manera que cada alumno pueda completar la especificación según su propio criterio.

Se valorará tanto la calidad de la aplicación desarrollada como el grado de conocimiento del alumno sobre la misma.

Serán requisitos mínimos para aprobar:

- La correcta compilación y generación del ejecutable, así como su correcto funcionamiento.
- La entrega de la memoria, todo el código fuente y, en su caso, el ejecutable.

Se valorarán positivamente (según el caso), es decir, servirán para subir nota:

- La incorporación de especificaciones a la aplicación que impliquen un mayor grado de dificultad en lo que se refiere a su interfaz.
- La utilización de varios tipos de controles, incluyendo los no utilizados en clase.
- La extensión de requisitos propuestos, en general, el uso de cualquier característica que haya implicado la investigación personal del alumno.