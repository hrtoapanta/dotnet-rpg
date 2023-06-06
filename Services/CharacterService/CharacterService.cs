global using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CharacterService(IMapper mapper, DataContext context)
        {
            _context =context;
           _mapper = mapper;
        }
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var character = _mapper.Map<Character>(newCharacter);

            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
           serviceResponse.Data =  await _context.Characters.Select(c=>_mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int Id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try{

            var character = await _context.Characters.FirstOrDefaultAsync(c=>c.Id==Id);
            if(character is null){
                throw new Exception($"Character with Id {Id} not found");
            }
           _context.Characters.Remove(character);
          await _context.SaveChangesAsync();
            
            serviceResponse.Data= await _context.Characters.Select(c=> _mapper.Map<GetCharacterDto>(c)).ToListAsync();
         
            }
            catch(Exception ex){
                serviceResponse.Success=false;
                serviceResponse.Message = ex.Message;
            }
             return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacter = await _context.Characters.ToListAsync();
            serviceResponse.Data = dbCharacter.Select(c=>_mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var dbCharacter = await _context.Characters.FirstOrDefaultAsync(c=>c.Id==id);
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateChartacterDto updatedCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try{

            var character = await _context.Characters.FirstOrDefaultAsync(c=>c.Id==updatedCharacter.Id);
            if(character is null){
                throw new Exception($"Character with Id {updatedCharacter.Id} not found");
            }
            //mapeo de objetos con dos opciones 
            // la primera opcion indicamos la clase destino y el objeto que queremos mapear
            //_mapper.Map<Character>(updatedCharacter);
            
            //esta segunda mapeamos indicando el objeto origen(updateCharacter) y el objecto destino(character)
            //pero ademas hay que crear una nueva entrada en el mapper profile idicando las clases que se se van a mapear
            _mapper.Map(updatedCharacter, character);
            character.Name = updatedCharacter.Name;
            character.HitPoints = updatedCharacter.HitPoints;
            character.Strength = updatedCharacter.Strength;
            character.Defense = updatedCharacter.Defense;
            character.Intelligence = updatedCharacter.Intelligence;
            character.Class = updatedCharacter.Class;
            
            await _context.SaveChangesAsync();
            serviceResponse.Data= _mapper.Map<GetCharacterDto>(character);
         
            }
            catch(Exception ex){
                serviceResponse.Success=false;
                serviceResponse.Message = ex.Message;
            }
             return serviceResponse;
        }
    }
}