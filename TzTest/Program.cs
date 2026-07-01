using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== TIMEZONE DIAGNOSTIC ===");
        
        var nowLocal = DateTime.Now;
        var nowUtc = DateTime.UtcNow;
        
        Console.WriteLine($"DateTime.Now: {nowLocal} (Kind: {nowLocal.Kind})");
        Console.WriteLine($"DateTime.UtcNow: {nowUtc} (Kind: {nowUtc.Kind})");
        Console.WriteLine($"TimeZoneInfo.Local: {TimeZoneInfo.Local.Id} (DisplayName: {TimeZoneInfo.Local.DisplayName}, BaseUtcOffset: {TimeZoneInfo.Local.BaseUtcOffset})");
        
        // Simular lo que llega del frontend
        string dateStr = "2026-07-01";
        string timeStr = "20:00"; // 8 PM
        
        var fechaCita = DateOnly.Parse(dateStr);
        var horaCita = TimeOnly.Parse(timeStr);
        var fechaHoraCita = fechaCita.ToDateTime(horaCita);
        
        Console.WriteLine($"\nSimulando Cita: Fecha={dateStr}, Hora={timeStr}");
        Console.WriteLine($"fechaHoraCita: {fechaHoraCita} (Kind: {fechaHoraCita.Kind})");
        
        TimeZoneInfo colombiaZone;
        try
        {
            colombiaZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
        }
        catch (TimeZoneNotFoundException)
        {
            colombiaZone = TimeZoneInfo.FindSystemTimeZoneById("America/Bogota");
        }
        
        Console.WriteLine($"colombiaZone: {colombiaZone.Id} (BaseUtcOffset: {colombiaZone.BaseUtcOffset})");
        
        var tTest = new TimeSpan(14, 30, 0);
        Console.WriteLine($"TimeSpan 14:30 formatted with hh\\:mm: {tTest.ToString(@"hh\:mm")}");
        
        var fechaHoraCitaLocal = DateTime.SpecifyKind(fechaHoraCita, DateTimeKind.Unspecified);
        var utcFechaHoraCita = TimeZoneInfo.ConvertTimeToUtc(fechaHoraCitaLocal, colombiaZone);
        
        Console.WriteLine($"utcFechaHoraCita: {utcFechaHoraCita} (Kind: {utcFechaHoraCita.Kind})");
        
        bool isPast = utcFechaHoraCita <= nowUtc;
        Console.WriteLine($"utcFechaHoraCita <= DateTime.UtcNow: {isPast}");
        
        // ¿Qué pasa si comparamos directamente local vs local?
        // Si el servidor local ya está en Colombia (SA Pacific Standard Time), entonces DateTime.Now es hora local de Colombia.
        // Y fechaHoraCita es hora local de Colombia.
        // Si comparamos fechaHoraCita <= DateTime.Now:
        bool isPastLocal = fechaHoraCita <= nowLocal;
        Console.WriteLine($"fechaHoraCita <= DateTime.Now (Local): {isPastLocal}");
    }
}
