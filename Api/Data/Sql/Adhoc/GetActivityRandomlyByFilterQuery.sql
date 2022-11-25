SELECT *
  FROM public.activity
 WHERE @fromDate <= date
   AND @toDate >= date
   AND @fromCapacity <= capacity
   AND @toCapacity >= capacity 
 ORDER BY random()
 LIMIT @count