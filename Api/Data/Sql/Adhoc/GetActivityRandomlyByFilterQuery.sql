SELECT *
  FROM public.activity
 WHERE @fromDate <= date
   AND @toDate >= date
   AND @fromCapacity <= capacity
   AND @toCapacity >= capacity 
   AND CASE 
          WHEN @key is not null THEN LOWER(title) LIKE CONCAT('%', LOWER(@key), '%')
          ELSE true
       END
 ORDER BY random()
 LIMIT @count