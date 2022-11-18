SELECT *
  FROM public.activity
 WHERE LOWER(title) LIKE CONCAT('%', LOWER(@key), '%')
 ORDER BY random()
 LIMIT @count