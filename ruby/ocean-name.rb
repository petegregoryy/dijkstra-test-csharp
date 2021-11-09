require "ocean_names"

puts OceanNames.reverse_geocode(lng: ARGV[1].to_f,lat:ARGV[0].to_f);